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

        public IActionResult ManageSubjects()
        {
            var groupedSubjects = _context.SubjectSelections
                .AsEnumerable()
                .GroupBy(s => s.Username)
                .ToList();

            ViewBag.Subjects = _context.SubjectSelections.ToList(); // All subject names used in dropdown

            return View(groupedSubjects);
        }

        // POST: Update Subject
        [HttpPost]
        public IActionResult UpdateSubject(int id, string subjectName, int creditPoints)
        {
            var subject = _context.SubjectSelections.FirstOrDefault(s => s.Id == id);
            if (subject == null)
            {
                TempData["Error"] = "Subject not found.";
                return RedirectToAction("ManageSubjects");
            }

            subject.SubjectName = subjectName;
            subject.CreditPoints = creditPoints;

            _context.SubjectSelections.Update(subject);
            _context.SaveChanges();

            TempData["Success"] = "Subject updated successfully.";
            return RedirectToAction("ManageSubjects");
        }

        // POST: Delete Subject
        [HttpPost]
        public IActionResult DeleteSubject(int id)
        {
            var subject = _context.SubjectSelections.FirstOrDefault(s => s.Id == id);
            if (subject != null)
            {
                _context.SubjectSelections.Remove(subject);
                _context.SaveChanges();
                TempData["Success"] = "Subject deleted successfully.";
            }
            return RedirectToAction("ManageSubjects");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("AdminEmail");
            return RedirectToAction("Login");
        }


    }
}
