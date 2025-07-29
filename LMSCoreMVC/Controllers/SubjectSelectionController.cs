using LMSCoreMVC.Data;
using LMSCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace LMSCoreMVC.Controllers
{
    public class SubjectSelectionController : Controller
    {
        private readonly LMSDbContext _context;

        public SubjectSelectionController(LMSDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login", "Account");

            var selectedSubjects = _context.SubjectSelections
                                           .Where(s => s.Username == username)
                                           .ToList();
            return View("~/Views/Subject/Index.cshtml", selectedSubjects);
        }

        [HttpPost]
        public IActionResult Select(string subjectName, int creditPoints)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login", "Account");

            var newSelection = new SubjectSelection
            {
                SubjectName = subjectName,
                CreditPoints = creditPoints,
                Username = username
            };

            _context.SubjectSelections.Add(newSelection);
            _context.SaveChanges();

            TempData["Success"] = "Subject selected successfully!";
            return RedirectToAction("Index");
        }

    }
}
