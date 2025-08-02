using LMSCoreMVC.Data;
using LMSCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMSCoreMVC.Controllers
{
    public class TestController : Controller
    {
        private readonly LMSDbContext _context;

        public TestController(LMSDbContext context)
        {
            _context = context;
        }

        // View all tests
        public async Task<IActionResult> Index()
        {
            var tests = await _context.Tests.OrderByDescending(t => t.TestDate).ToListAsync();
            return View(tests);
        }

        // Show form to add new test
        public IActionResult Create()
        {
            return View();
        }

        // Handle form POST
        [HttpPost]
        public async Task<IActionResult> Create(Test test)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fill all required fields.";
                return View(test);
            }

            _context.Tests.Add(test);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Test added successfully!";
            return RedirectToAction("Index");
        }

        //delete test

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var test = await _context.Tests.FindAsync(id);
            if (test == null)
            {
                TempData["Error"] = "Test not found.";
                return RedirectToAction("Index");
            }

            _context.Tests.Remove(test);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Test deleted successfully!";
            return RedirectToAction("Index");
        }

    }
}
