using Microsoft.AspNetCore.Mvc;

namespace LMSCoreMVC.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("JWT")))
                return RedirectToAction("Login", "Account");

            return View();
        }
    }
}
