using LMSCoreMVC.Data;
using LMSCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class SubjectsController : Controller
{
    private readonly LMSDbContext _context;

    public SubjectsController(LMSDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var allSubjects = await _context.Subjects.ToListAsync();
        var username = HttpContext.Session.GetString("Username");

        var selectedSubjects = await _context.StudentSubjects
            .Include(s => s.Subject)
            .Where(s => s.StudentUsername == username)
            .ToListAsync();

        ViewBag.AllSubjects = allSubjects;
        return View(selectedSubjects);
    }

    [HttpPost]
    public async Task<IActionResult> AddSubject(int subjectId)
    {
        var username = HttpContext.Session.GetString("Username");

        if (!_context.StudentSubjects.Any(s => s.StudentUsername == username && s.SubjectId == subjectId))
        {
            var studentSubject = new StudentSubjects
            {
                StudentUsername = username,
                SubjectId = subjectId
            };

            _context.StudentSubjects.Add(studentSubject);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Index");
    }
}
