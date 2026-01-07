using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;          // ✅ ADD THIS (for Session.GetString)
using LibraryManagementSystem.Helper;

namespace LibraryManagementSystem.Service
{
    public class PremiumHandler : AuthorizationHandler<PremiumRequirement>
    {
        private readonly ISignInHelper _signInHelper;
        private readonly IPremiumAccessService _premiumService;
        private readonly IHttpContextAccessor _http;

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
            // must be student
            if (!context.User.IsInRole("Student"))
                return;

            var studentId = _signInHelper.UserId ?? 0;
            if (studentId <= 0)
                return;

            // must have purchased
            var purchased = await _premiumService.HasPurchasedMembershipAsync(studentId, CancellationToken.None);
            if (!purchased)
                return;

            // must unlock via barcode (session)
            var unlocked = _http.HttpContext?.Session?.GetString(PremiumSessionKey);
            if (unlocked == "1")
            {
                context.Succeed(requirement);
            }
        }
    }
}
