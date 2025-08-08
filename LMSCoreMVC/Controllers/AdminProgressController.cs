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



        private int CalculateAttendance(string studentName)
        {
            var totalDays = _context.Attendance.Count(a => a.StudentName == studentName);
            var presentDays = _context.Attendance.Count(a => a.StudentName == studentName && a.Status == "Present");

            return totalDays > 0 ? (presentDays * 100 / totalDays) : 0;
        }
    }
}
