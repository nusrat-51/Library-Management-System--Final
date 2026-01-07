using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using static LibraryManagementSystem.Auth_IdentityModel.IdentityModel;

namespace LibraryManagementSystem.Service
{
    public class RoleClaimsTransformation : IClaimsTransformation
    {
        private readonly UserManager<User> _userManager;

        public RoleClaimsTransformation(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if (principal?.Identity?.IsAuthenticated != true)
                return principal;

            var identity = principal.Identity as ClaimsIdentity;
            if (identity == null)
                return principal;

            // If roles already exist, do nothing
            if (identity.Claims.Any(c => c.Type == ClaimTypes.Role))
                return principal;

            var userId = _userManager.GetUserId(principal);
            if (string.IsNullOrWhiteSpace(userId))
                return principal;

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return principal;

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var r in roles)
                identity.AddClaim(new Claim(ClaimTypes.Role, r));

            return principal;
        }
    }
}
