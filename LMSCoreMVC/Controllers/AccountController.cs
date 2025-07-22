using LMSCoreMVC.Data;
using LMSCoreMVC.Models;
using LMSCoreMVC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMSCoreMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly LMSDbContext _context;
        private readonly JwtService _jwtService;

        public AccountController(LMSDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login");
            }
            return View(user);
        }

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
            if (user == null)
            {
                ViewBag.Error = "Invalid credentials.";
                return View();
            }

            var token = _jwtService.GenerateToken(user);
            HttpContext.Session.SetString("JWT", token);
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("Email", user.Email);
            return RedirectToAction("Index", "Dashboard");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
