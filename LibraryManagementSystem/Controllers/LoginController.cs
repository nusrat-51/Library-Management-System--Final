using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet("/Login")]
        public IActionResult Index()
        {
            // ✅ Redirect to your existing login page
            // If your login controller is AccountController and action is Login:
            return RedirectToAction("Login", "Account");

            // If your login controller/action is different, change above line only.
        }
    }
}
