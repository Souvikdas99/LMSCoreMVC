using LMSCoreMVC.Data;
using LMSCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace LMSCoreMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly LMSDbContext _context;

        public AdminController(LMSDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Admin admin)
        {
            if (ModelState.IsValid)
            {
                _context.Admins.Add(admin);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Admin registered successfully!";
                return RedirectToAction("Login");
            }

            return View(admin);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var admin = _context.Admins.FirstOrDefault(a => a.Email == email && a.Password == password);

            if (admin != null)
            {
                HttpContext.Session.SetString("AdminEmail", admin.Email);
                return RedirectToAction("Dashboard");
            }

            ViewBag.Error = "Invalid credentials";
            return View();
        }

        public IActionResult Dashboard()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminEmail")))
                return RedirectToAction("Login");

            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("AdminEmail");
            return RedirectToAction("Login");
        }
    }
}
