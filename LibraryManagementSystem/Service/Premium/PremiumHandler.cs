using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using LibraryManagementSystem.Helper;

namespace LibraryManagementSystem.Service
{
    public class PremiumHandler : AuthorizationHandler<PremiumRequirement>
    {
        private readonly ISignInHelper _signInHelper;
        private readonly IPremiumAccessService _premiumService;
        private readonly IHttpContextAccessor _http;

        // ✅ Single source of truth for session key
        public const string PremiumSessionKey = "PREMIUM_UNLOCKED";

        public PremiumHandler(
            ISignInHelper signInHelper,
            IPremiumAccessService premiumService,
            IHttpContextAccessor http)
        {
            _signInHelper = signInHelper;
            _premiumService = premiumService;
            _http = http;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PremiumRequirement requirement)
        {
            // ✅ Only students can pass this policy
            if (!context.User.IsInRole("Student"))
                return;

            // ✅ Get studentId reliably (SignInHelper first, then Claim fallback)
            long studentId = (long)(_signInHelper.UserId ?? 0);

            if (studentId <= 0)
            {
                var claimId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!string.IsNullOrWhiteSpace(claimId) && long.TryParse(claimId, out var parsed))
                {
                    studentId = parsed;
                }
            }

            if (studentId <= 0)
                return;

            // ✅ Session check
            var httpCtx = _http.HttpContext;
            var unlocked = httpCtx?.Session?.GetString(PremiumSessionKey);

            if (unlocked == "1")
            {
                context.Succeed(requirement);
                return;
            }

            // ✅ DB check (membership purchased)
            var purchased = await _premiumService.HasPurchasedMembershipAsync(studentId, CancellationToken.None);
            if (purchased)
            {
                // ✅ IMPORTANT FIX:
                // purchased means student should be treated as unlocked in the whole app.
                // So we set session too (keeps BookApplicationController + Views consistent).
                if (httpCtx?.Session != null)
                {
                    httpCtx.Session.SetString(PremiumSessionKey, "1");
                }

                context.Succeed(requirement);
                return;
            }

            // ❌ Not unlocked + not purchased -> do nothing (policy fails)
        }
    }
}
