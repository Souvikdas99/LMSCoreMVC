using LMSCoreMVC.Data;
using LMSCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMSCoreMVC.Controllers
{
    public class AdminTestController : Controller
    {
        private readonly LMSDbContext _context;
        public AdminTestController(LMSDbContext context) { _context = context; }

        public IActionResult Index()
        {
            var tests = _context.Tests.Include(t => t.Questions).ToList();
            return View(tests);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Test model)
        {
            if (!ModelState.IsValid) return View(model);
            _context.Tests.Add(model);
            _context.SaveChanges();
            return RedirectToAction("EditQuestions", new { testId = model.Id });
        }

        public IActionResult EditQuestions(int testId)
        {
            var test = _context.Tests.Include(t => t.Questions).FirstOrDefault(t => t.Id == testId);
            if (test == null) return NotFound();
            return View(test);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddQuestion(TestQuestion q)
        {
            if (!ModelState.IsValid) return RedirectToAction("EditQuestions", new { testId = q.TestId });
            _context.TestQuestions.Add(q);
            _context.SaveChanges();
            return RedirectToAction("EditQuestions", new { testId = q.TestId });
        }

        public IActionResult Assign(int testId)
        {
            var test = _context.Tests.Find(testId);
            var students = _context.Users.Where(u => u.Role == "Student").ToList();
            ViewBag.Test = test;
            return View(students);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Assign(int testId, int[] selectedStudentIds)
        {
            foreach (var sid in selectedStudentIds ?? new int[0])
            {
                bool exists = _context.StudentTests.Any(s => s.TestId == testId && s.UserId == sid);
                if (!exists)
                {
                    _context.StudentTests.Add(new StudentTest { TestId = testId, UserId = sid, AssignedDate = DateTime.UtcNow, Status = "Pending" });
                }
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Submissions(int testId)
        {
            ViewBag.Test = _context.Tests.Find(testId);
            var list = _context.StudentTests.Include(st => st.Student)
                                            .Where(st => st.TestId == testId)
                                            .ToList();
            return View(list);
        }

        public IActionResult Grade(int studentTestId)
        {
            var st = _context.StudentTests
                .Include(s => s.Student)
                .Include(s => s.Test).ThenInclude(t => t.Questions)
                .Include(s => s.Submissions).ThenInclude(ss => ss.Question)
                .FirstOrDefault(x => x.Id == studentTestId);

            if (st == null) return NotFound();
            return View(st);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveGrade(int studentTestId, int score)
        {
            var st = _context.StudentTests.Find(studentTestId);
            if (st == null) return NotFound();
            st.Score = score;
            st.Status = "Submitted";
            st.SubmittedDate = st.SubmittedDate ?? DateTime.UtcNow;
            _context.SaveChanges();
            return RedirectToAction("Submissions", new { testId = st.TestId });
        }
    }
}
