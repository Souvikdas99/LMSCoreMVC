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

        // GET: Assignment
        public async Task<IActionResult> Index()
        {
            var assignments = await _context.Assignments.ToListAsync();
            return View(assignments);
        }

        // POST: Assignment/Submit
        [HttpPost]
        public async Task<IActionResult> Submit(Assignment assignment, IFormFile file)
        {
            try
            {
                if (!ModelState.IsValid || file == null || file.Length == 0)
                {
                    TempData["Success"] = "Invalid form submission or missing file.";
                    return RedirectToAction("Index");
                }

                if (file.Length > 10 * 1024 * 1024) // 10 MB limit
                {
                    TempData["Success"] = "File size exceeds 10 MB.";
                    return RedirectToAction("Index");
                }

                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                assignment.FilePath = "/uploads" + uniqueFileName;

                _context.Assignments.Add(assignment);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Assignment submitted successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Success"] = $"Error occurred: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        public IActionResult ViewFile(string path)
        {
            var fullPath = Path.Combine(_env.WebRootPath, path.TrimStart('/'));

            if (!System.IO.File.Exists(fullPath))
                return NotFound();

            var fileBytes = System.IO.File.ReadAllBytes(fullPath);
            var contentType = "application/octet-stream";

            return File(fileBytes, contentType, Path.GetFileName(fullPath));
        }
    }
}
