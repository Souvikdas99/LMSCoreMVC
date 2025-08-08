using LMSCoreMVC.Data;
using LMSCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace LMSCoreMVC.Controllers
{
    public class AdminProgressController : Controller
    {
        private readonly LMSDbContext _context;

        public AdminProgressController(LMSDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var allStudents = _context.Assignment
                .Select(a => a.StudentName)
                .Distinct()
                .ToList();

            var studentProgress = allStudents.Select(name => new
            {
                StudentName = name,
                TotalAssignments = _context.Assignment.Count(a => a.StudentName == name),
                Accepted = _context.Assignment.Count(a => a.StudentName == name && a.Status == "Accepted"),
                Rejected = _context.Assignment.Count(a => a.StudentName == name && a.Status == "Rejected"),
                Pending = _context.Assignment.Count(a => a.StudentName == name && a.Status == "Pending"),
                AttendancePercentage = CalculateAttendance(name),
                AverageScore = 0 // Placeholder until test system is added
            }).ToList();

            ViewBag.StudentProgress = studentProgress;
            return View();
        }

        private int CalculateAttendance(string studentName)
        {
            var totalDays = _context.Attendance.Count(a => a.StudentName == studentName);
            var presentDays = _context.Attendance.Count(a => a.StudentName == studentName && a.Status == "Present");

            return totalDays > 0 ? (presentDays * 100 / totalDays) : 0;
        }
    }
}
