using LMSCoreMVC.Data;
using LMSCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LMSCoreMVC.Controllers
{
    public class SubjectSelectionController : Controller
    {
        private readonly LMSDbContext _context;

        public SubjectSelectionController(LMSDbContext context)
        {
            _context = context;
        }

        // Show selected subjects for a student
        public async Task<IActionResult> Index()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login", "Account");

            var selectedSubjects = await _context.SubjectSelections
                                                 .Where(s => s.Username == username)
                                                 .ToListAsync();

            return View(selectedSubjects);
        }

        // GET: Show subject selection form (you can create a view for this)
        public IActionResult Select()
        {
            return View();
        }

        // POST: Handle subject selection
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Select(string subjectName, int creditPoints)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login", "Account");

            var alreadySelected = await _context.SubjectSelections
                .AnyAsync(s => s.Username == username && s.SubjectName == subjectName);

            if (alreadySelected)
            {
                TempData["Error"] = "Subject already selected.";
                return RedirectToAction("Index");
            }

            var newSelection = new SubjectSelection
            {
                SubjectName = subjectName,
                CreditPoints = creditPoints,
                Username = username
            };

            _context.SubjectSelections.Add(newSelection);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Subject selected successfully!";
            return RedirectToAction("Index");
        }

        // DELETE: Remove selected subject
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int id)
        {
            var subject = await _context.SubjectSelections.FindAsync(id);
            if (subject != null)
            {
                _context.SubjectSelections.Remove(subject);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Subject removed.";
            }
            else
            {
                TempData["Error"] = "Subject not found.";
            }

            return RedirectToAction("Index");
        }

        // ADMIN: View all students' subject selections
        public async Task<IActionResult> Manage()
        {
            var grouped = await _context.SubjectSelections
                .GroupBy(s => s.Username)
                .Select(g => new
                {
                    Username = g.Key,
                    Subjects = g.Select(s => s.SubjectName).ToList(),
                    TotalCredits = g.Sum(s => s.CreditPoints)
                })
                .ToListAsync();

            ViewBag.GroupedSubjects = grouped;
            return View(); // You can use a separate admin view here
        }
    }
}
