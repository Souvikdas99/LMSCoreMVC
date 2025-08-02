using LMSCoreMVC.Data;
using LMSCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace LMSCoreMVC.Controllers
{
    public class ProgressController : Controller
    {
        private readonly LMSDbContext _context;

        public ProgressController(LMSDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var studentName = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(studentName))
                return RedirectToAction("Login", "Account");

            // 1. Assignment Progress
            var totalAssignments = _context.Assignment.Count(a => a.StudentName == studentName);
            var accepted = _context.Assignment.Count(a => a.StudentName == studentName && a.Status == "Accepted");
            var rejected = _context.Assignment.Count(a => a.StudentName == studentName && a.Status == "Rejected");
            var pending = totalAssignments - accepted - rejected;

            // 2. Test Score Average
            var scores = _context.TestResults.Where(t => t.StudentName == studentName).Select(t => t.Score).ToList();
            var averageScore = scores.Any() ? scores.Average() : 0;

            // 3. Attendance Percentage
            var totalDays = _context.Attendance.Count(a => a.StudentName == studentName);
            var presentDays = _context.Attendance.Count(a => a.StudentName == studentName && a.Status == "Present");
            var attendancePercentage = totalDays > 0 ? (presentDays * 100 / totalDays) : 0;

            // Store in ViewBag
            ViewBag.TotalAssignments = totalAssignments;
            ViewBag.Accepted = accepted;
            ViewBag.Rejected = rejected;
            ViewBag.Pending = pending;
            ViewBag.AverageScore = averageScore;
            ViewBag.Attendance = attendancePercentage;

            return View();
        }
    }
}
