using CatMS;
using LibraryManagementSystem.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static LibraryManagementSystem.Auth_IdentityModel.IdentityModel;

namespace LibraryManagementSystem.Controllers
{

    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IAuthService _authService;
        public AccountController(
          SignInManager<User> signInManager,
          UserManager<User> userManager,
          IAuthService authService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _authService = authService;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _authService.Register(model);

            if (!result.Success)
            {
                result.Errors.ForEach(e => ModelState.AddModelError("", e));
                return View(model);
            }

            var user = await _signInManager.UserManager
                .FindByIdAsync(result.UserId.ToString());

            if (user != null)
            {
                await _signInManager.SignInAsync(user, false);
            }
            return RedirectToAction("Index", "Dashboard");

            
        }



        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
            if (result.Succeeded)
                return LocalRedirect("/Dashboard/Index");
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");

        }




        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");
            return View(user);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Settings()
        {
            return View();
        }
        [Authorize]
        [HttpGet("ChangePassword")]
        public IActionResult ChangePassword()
        {
            return View();
        }
    }
}
