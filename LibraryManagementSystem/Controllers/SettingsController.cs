using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    public class SettingsController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Settings";
            ViewData["ActivePage"] = "Settings";
            return View();
        }
    }
}
