using LMSCoreMVC.Data;
using LMSCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace LMSCoreMVC.Controllers
{
    public class AdminAssignmentController : Controller
    {
        private readonly LMSDbContext _context;

        public AdminAssignmentController(LMSDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var assignments = _context.Assignment.ToList();
            return View(assignments);
        }

        [HttpPost]
        public IActionResult UpdateStatus(int id, string status)
        {
            var assignment = _context.Assignment.FirstOrDefault(a => a.Id == id);
            if (assignment != null)
            {
                assignment.Status = status;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
