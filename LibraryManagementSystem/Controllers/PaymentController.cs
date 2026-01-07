using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace LibraryManagementSystem.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public PaymentController(ApplicationDbContext context, IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        // -------------------------------
        // STEP A: create FinePayment record per application
        // -------------------------------
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PayFine(int bookApplicationId, CancellationToken cancellationToken)
        {
            var app = await _context.bookApplications
                .FirstOrDefaultAsync(x => x.Id == bookApplicationId, cancellationToken);

            if (app == null) return NotFound();

            // ✅ Always calculate fine from ReturnDate (DB FineAmount may be 0 because you calculate only in UI)
            decimal finePerDay = 10;

            var today = DateTime.Today;
            var dueDate = app.ReturnDate.Date;

            decimal calculatedAmount = 0;
            if (dueDate != default && dueDate < today)
            {
                var overdueDays = (today - dueDate).Days;
                if (overdueDays > 0)
                    calculatedAmount = overdueDays * finePerDay;
            }

            decimal amount = app.FineAmount > 0 ? app.FineAmount : calculatedAmount;

            if (amount <= 0)
            {
                TempData["Error"] = "No fine due for this record.";
                return RedirectToAction("Index", "BookApplication");
            }

            // ✅ If already paid, go to receipt/result
            var alreadyPaid = await _context.FinePayments
                .FirstOrDefaultAsync(p => p.BookApplicationId == bookApplicationId && p.Status == "Paid", cancellationToken);

            if (alreadyPaid != null)
                return RedirectToAction("Result", new { tran_id = alreadyPaid.TranId });

            // If already has unpaid payment, reuse it (your logic kept)
            var existing = await _context.FinePayments
                .FirstOrDefaultAsync(p => p.BookApplicationId == bookApplicationId && p.Status == "Initiated", cancellationToken);

            if (existing != null)
                return RedirectToAction("StartGateway", new { tranId = existing.TranId });

            var tranId = Guid.NewGuid().ToString("N");

            var payment = new FinePayment
            {
                BookApplicationId = bookApplicationId,
                StudentId = app.StudentId,
                StudentEmail = app.StudentEmail,
                Amount = amount,
                TranId = tranId,
                Status = "Initiated",
                Gateway = "SSLCommerz",
                CreatedAt = DateTime.Now
            };

            _context.FinePayments.Add(payment);
            await _context.SaveChangesAsync(cancellationToken);

            return RedirectToAction("StartGateway", new { tranId });
        }

        // -------------------------------
        // ✅ CASH PAYMENT (Manager/Admin/Librarian side)
        // -------------------------------
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PayCash(int bookApplicationId, CancellationToken cancellationToken)
        {
            var app = await _context.bookApplications
                .FirstOrDefaultAsync(x => x.Id == bookApplicationId, cancellationToken);

            if (app == null) return NotFound();

            decimal finePerDay = 10;
            var today = DateTime.Today;
            var dueDate = app.ReturnDate.Date;

            decimal calculatedAmount = 0;
            if (dueDate != default && dueDate < today)
            {
                var overdueDays = (today - dueDate).Days;
                if (overdueDays > 0)
                    calculatedAmount = overdueDays * finePerDay;
            }

            decimal amount = app.FineAmount > 0 ? app.FineAmount : calculatedAmount;

            if (amount <= 0)
            {
                TempData["Error"] = "No fine due for this record.";
                return RedirectToAction("Index", "BookApplication");
            }

            // If already paid, show receipt
            var alreadyPaid = await _context.FinePayments
                .FirstOrDefaultAsync(p => p.BookApplicationId == bookApplicationId && p.Status == "Paid", cancellationToken);

            if (alreadyPaid != null)
                return RedirectToAction("Receipt", new { tran_id = alreadyPaid.TranId });

            var tranId = Guid.NewGuid().ToString("N");

            var payment = new FinePayment
            {
                BookApplicationId = bookApplicationId,
                StudentId = app.StudentId,
                StudentEmail = app.StudentEmail,
                Amount = amount,
                TranId = tranId,
                Status = "Paid",
                Gateway = "Cash",
                ValId = "CASH",
                CreatedAt = DateTime.Now,
                PaidAt = DateTime.Now
            };

            _context.FinePayments.Add(payment);

            // keep your "fine cleared" behavior
            app.FineAmount = 0;

            await _context.SaveChangesAsync(cancellationToken);

            return RedirectToAction("Receipt", new { tran_id = tranId });
        }

        // -------------------------------
        // ✅ Cash receipt page
        // -------------------------------
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Receipt(string tran_id, CancellationToken cancellationToken)
        {
            var payment = await _context.FinePayments
                .FirstOrDefaultAsync(p => p.TranId == tran_id, cancellationToken);

            if (payment == null) return NotFound();

            return View("CashReceipt", payment);
        }

        // -------------------------------
        // STEP B: Create session and redirect user to SSLCOMMERZ Hosted Page
        // -------------------------------
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> StartGateway(string tranId, CancellationToken cancellationToken)
        {
            var payment = await _context.FinePayments.FirstOrDefaultAsync(p => p.TranId == tranId, cancellationToken);
            if (payment == null) return NotFound();

            if (payment.Status == "Paid")
                return RedirectToAction("Result", new { tran_id = payment.TranId });

            var storeId = _config["SSLCOMMERZ:StoreId"];
            var storePass = _config["SSLCOMMERZ:StorePassword"];
            var sessionUrl = _config["SSLCOMMERZ:SandboxSessionUrl"];
            var currency = _config["SSLCOMMERZ:Currency"] ?? "BDT";

            // ✅ This MUST be public URL (ngrok) so SSLCommerz can redirect back
            var publicBaseUrl = _config["SSLCOMMERZ:PublicBaseUrl"];
            if (string.IsNullOrWhiteSpace(publicBaseUrl))
                return Content("PublicBaseUrl missing in appsettings.json (SSLCOMMERZ:PublicBaseUrl)");

            // cleanup (important)
            publicBaseUrl = publicBaseUrl.Split("->")[0].Trim();
            publicBaseUrl = publicBaseUrl.TrimEnd('/');

            var successUrl = $"{publicBaseUrl}/Payment/Success";
            var failUrl = $"{publicBaseUrl}/Payment/Fail";
            var cancelUrl = $"{publicBaseUrl}/Payment/Cancel";
            var ipnUrl = $"{publicBaseUrl}/Payment/Ipn";

            var postData = new Dictionary<string, string>
            {
                ["store_id"] = storeId ?? "",
                ["store_passwd"] = storePass ?? "",
                ["total_amount"] = payment.Amount.ToString("0.00"),
                ["currency"] = currency,
                ["tran_id"] = payment.TranId,

                ["success_url"] = successUrl,
                ["fail_url"] = failUrl,
                ["cancel_url"] = cancelUrl,
                ["ipn_url"] = ipnUrl,

                ["cus_name"] = payment.StudentEmail,
                ["cus_email"] = payment.StudentEmail,
                ["cus_add1"] = "Dhaka",
                ["cus_city"] = "Dhaka",
                ["cus_country"] = "Bangladesh",
                ["cus_phone"] = "01700000000",

                ["shipping_method"] = "NO",
                ["product_name"] = "Library Fine Payment",
                ["product_category"] = "Fine",
                ["product_profile"] = "general"
            };

            if (string.IsNullOrWhiteSpace(sessionUrl) || !Uri.IsWellFormedUriString(sessionUrl, UriKind.Absolute))
                return Content("Session URL invalid. Check SSLCOMMERZ:SandboxSessionUrl in appsettings.json");

            var client = _httpClientFactory.CreateClient();

            var response = await client.PostAsync(sessionUrl, new FormUrlEncodedContent(postData), cancellationToken);
            var json = await response.Content.ReadAsStringAsync(cancellationToken);

            // ✅ IMPORTANT: show SSLCommerz errors clearly
            if (!response.IsSuccessStatusCode)
            {
                return Content($"SSLCOMMERZ HTTP ERROR: {(int)response.StatusCode}\n\n{json}");
            }

            using var doc = JsonDocument.Parse(json);

            if (!doc.RootElement.TryGetProperty("GatewayPageURL", out var gatewayUrlEl))
                return Content("SSLCOMMERZ session failed: " + json);

            var gatewayUrl = gatewayUrlEl.GetString();
            if (string.IsNullOrWhiteSpace(gatewayUrl))
                return Content("SSLCOMMERZ GatewayPageURL missing: " + json);

            return Redirect(gatewayUrl);
        }

        // -------------------------------
        // CALLBACKS (must be public - ngrok)
        // -------------------------------
        [HttpPost, HttpGet]
        public async Task<IActionResult> Success(string tran_id, string val_id, CancellationToken cancellationToken)
        {
            var payment = await _context.FinePayments.FirstOrDefaultAsync(p => p.TranId == tran_id, cancellationToken);
            if (payment == null) return NotFound();

            payment.ValId = val_id;

            var storeId = _config["SSLCOMMERZ:StoreId"];
            var storePass = _config["SSLCOMMERZ:StorePassword"];
            var validationUrl = _config["SSLCOMMERZ:SandboxValidationUrl"];

            if (!string.IsNullOrWhiteSpace(validationUrl))
            {
                var url = $"{validationUrl}?val_id={val_id}&store_id={storeId}&store_passwd={storePass}&format=json";
                var client = _httpClientFactory.CreateClient();
                await client.GetAsync(url, cancellationToken);
            }

            payment.Status = "Paid";
            payment.PaidAt = DateTime.Now;

            var app = await _context.bookApplications.FirstOrDefaultAsync(x => x.Id == payment.BookApplicationId, cancellationToken);
            if (app != null) app.FineAmount = 0;

            await _context.SaveChangesAsync(cancellationToken);

            var localBaseUrl = _config["SSLCOMMERZ:LocalBaseUrl"] ?? "http://localhost:5086";
            localBaseUrl = localBaseUrl.Split("->")[0].Trim().TrimEnd('/');

            return Redirect($"{localBaseUrl}/Payment/Result?tran_id={tran_id}");
        }

        [HttpPost, HttpGet]
        public async Task<IActionResult> Fail(string tran_id, CancellationToken cancellationToken)
        {
            var payment = await _context.FinePayments.FirstOrDefaultAsync(p => p.TranId == tran_id, cancellationToken);
            if (payment != null)
            {
                payment.Status = "Failed";
                await _context.SaveChangesAsync(cancellationToken);
            }

            var localBaseUrl = _config["SSLCOMMERZ:LocalBaseUrl"] ?? "http://localhost:5086";
            localBaseUrl = localBaseUrl.Split("->")[0].Trim().TrimEnd('/');

            return Redirect($"{localBaseUrl}/Payment/Result?tran_id={tran_id}");
        }

        [HttpPost, HttpGet]
        public async Task<IActionResult> Cancel(string tran_id, CancellationToken cancellationToken)
        {
            var payment = await _context.FinePayments.FirstOrDefaultAsync(p => p.TranId == tran_id, cancellationToken);
            if (payment != null)
            {
                payment.Status = "Cancelled";
                await _context.SaveChangesAsync(cancellationToken);
            }

            var localBaseUrl = _config["SSLCOMMERZ:LocalBaseUrl"] ?? "http://localhost:5086";
            localBaseUrl = localBaseUrl.Split("->")[0].Trim().TrimEnd('/');

            return Redirect($"{localBaseUrl}/Payment/Result?tran_id={tran_id}");
        }

        // -------------------------------
        // RESULT PAGE
        // -------------------------------
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Result(string tran_id, CancellationToken cancellationToken)
        {
            var payment = await _context.FinePayments.FirstOrDefaultAsync(p => p.TranId == tran_id, cancellationToken);
            if (payment == null) return NotFound();

            if (payment.Status == "Paid") return View("PaymentSuccess", payment);
            if (payment.Status == "Failed") return View("PaymentFailed");
            if (payment.Status == "Cancelled") return View("PaymentCancelled");

            return View("PaymentFailed");
        }

        [HttpPost]
        public IActionResult Ipn()
        {
            return Ok();
        }
    }
}
