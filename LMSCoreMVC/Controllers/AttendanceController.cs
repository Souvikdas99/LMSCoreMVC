using LMSCoreMVC.Data;
using LMSCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMSCoreMVC.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly LMSDbContext _context;

        public AttendanceController(LMSDbContext context)
        {
            _context = context;
        }

        //  Show all attendance records
        public async Task<IActionResult> AllRecords()
        {
            var records = await _context.Attendance.OrderByDescending(a => a.Date).ToListAsync();
            return View(records);
        }

        //  Show form to mark attendance
        public IActionResult Mark()
        {
            return View();
        }

        //  Handle form post
        [HttpPost]
        public async Task<IActionResult> Mark(Attendance attendance)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fill in all required fields.";
                return View(attendance);
            }

            bool alreadyMarked = await _context.Attendance.AnyAsync(a =>
                a.StudentName == attendance.StudentName &&
                a.Date == attendance.Date);

            if (alreadyMarked)
            {
                TempData["Error"] = "You already marked attendance for this date.";
                return View(attendance);
            }

            _context.Attendance.Add(attendance);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Attendance marked successfully!";
            return RedirectToAction("Mark");
        }

        //date range 
        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Attendance.AsQueryable();

            if (startDate.HasValue)
                query = query.Where(a => a.Date >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(a => a.Date <= endDate.Value);

            var filteredRecords = await query.OrderByDescending(a => a.Date).ToListAsync();

            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");

            return View(filteredRecords);
        }

    }
}
