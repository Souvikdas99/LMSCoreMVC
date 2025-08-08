using LMSCoreMVC.Data;
using LMSCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMSCoreMVC.Controllers
{
    public class DashboardController : Controller
    {
        private readonly LMSDbContext _context;

        public DashboardController(LMSDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Check if user is authenticated via JWT token
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("JWT")))
                return RedirectToAction("Login", "Account");

            // Get the logged-in student's name from session
            var studentName = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(studentName))
                studentName = "Student";

            // Get total number of assignments submitted by student
            var totalAssignments = await _context.Assignment
                .CountAsync(a => a.StudentName == studentName);

            // Get total attendance marked as present
            var attendanceCount = await _context.Attendance
                .CountAsync(a => a.StudentName == studentName && a.IsPresent);

            // Pass data to the view
            var viewModel = new StudentDashboardViewModel
            {
                StudentName = studentName,
                TotalAssignments = totalAssignments,
                AttendanceCount = attendanceCount
            };

            return View(viewModel);
        }
    }
}
