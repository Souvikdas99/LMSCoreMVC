using LMSCoreMVC.Data;
using LMSCoreMVC.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMSCoreMVC.Controllers
{
    public class AssignmentController : Controller
    {
        private readonly LMSDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AssignmentController(LMSDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // List all assignments
        public async Task<IActionResult> Index()
        {
            var assignments = await _context.Assignment.OrderByDescending(a => a.SubmittedAt).ToListAsync();
            return View(assignments);
        }

        // Show submit form
        public IActionResult Submit()
        {
            return View();
        }

        // Handle submission
        [HttpPost]
        public async Task<IActionResult> Submit(Assignment assignment, IFormFile file)
        {
            if (!ModelState.IsValid || file == null || file.Length == 0)
            {
                TempData["Error"] = "Please fill in all required fields and select a file to upload.";
                return View(assignment);
            }

            if (file.Length > 10 * 1024 * 1024)
            {
                TempData["Error"] = "File size exceeds 10 MB.";
                return View(assignment);
            }

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            assignment.FilePath = "/uploads/" + fileName;
            assignment.SubmittedAt = DateTime.Now;
            assignment.Status = "Pending";

            _context.Assignment.Add(assignment);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Assignment submitted successfully!";
            return RedirectToAction("Index");
        }

        // Accept or Reject assignment
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var assignment = await _context.Assignment.FindAsync(id);
            if (assignment == null) return NotFound();

            if (status == "Accepted" || status == "Rejected")
            {
                assignment.Status = status;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        // Download file
        public IActionResult ViewFile(string path)
        {
            var fullPath = Path.Combine(_env.WebRootPath, path.TrimStart('/'));
            if (!System.IO.File.Exists(fullPath)) return NotFound();

            var fileBytes = System.IO.File.ReadAllBytes(fullPath);
            return File(fileBytes, "application/octet-stream", Path.GetFileName(fullPath));
        }
    }
}
