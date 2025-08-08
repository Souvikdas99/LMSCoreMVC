using LMSCoreMVC.Data;
using LMSCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMSCoreMVC.Controllers
{
    public class StudentTestController : Controller
    {
        private readonly LMSDbContext _context;
        public StudentTestController(LMSDbContext context) { _context = context; }

        public IActionResult MyTests()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username)) return RedirectToAction("Login", "Account");

            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null) return RedirectToAction("Login", "Account");

            var tests = _context.StudentTests
                        .Include(st => st.Test)
                        .Where(st => st.UserId == user.Id)
                        .ToList();

            return View(tests);
        }

        [HttpGet]
        public IActionResult Begin(int studentTestId)
        {
            var st = _context.StudentTests.Include(s => s.Test).FirstOrDefault(s => s.Id == studentTestId);
            if (st == null) return NotFound();
            if (st.Status == "Submitted") return RedirectToAction("MyTests");
            return View(st); // prompts username confirmation
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BeginConfirmed(int studentTestId, string username)
        {
            var st = _context.StudentTests.Include(s => s.Test).ThenInclude(t => t.Questions).FirstOrDefault(s => s.Id == studentTestId);
            if (st == null) return NotFound();

            var sessionName = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(sessionName) || sessionName != username)
            {
                TempData["Error"] = "Username mismatch or not logged in.";
                return RedirectToAction("Begin", new { studentTestId });
            }

            // time window check (convert ScheduledAt to UTC if stored in UTC)
            var now = DateTime.UtcNow;
            var start = st.Test.ScheduledAt.ToUniversalTime();
            var end = start.AddMinutes(st.Test.DurationInMinutes);
            if (now < start) { TempData["Error"] = "Test hasn't started yet."; return RedirectToAction("MyTests"); }
            if (now > end) { TempData["Error"] = "Test time window has passed."; return RedirectToAction("MyTests"); }

            ViewBag.StudentTest = st;
            return View("Take", st.Test);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitAnswers(int studentTestId)
        {
            var st = await _context.StudentTests.Include(s => s.Test).FirstOrDefaultAsync(s => s.Id == studentTestId);
            if (st == null) return NotFound();

            // delete previous answers if any
            var existing = _context.TestSubmissions.Where(a => a.StudentTestId == st.Id);
            _context.TestSubmissions.RemoveRange(existing);

            var posted = Request.Form;
            var questions = _context.TestQuestions.Where(q => q.TestId == st.TestId).ToList();
            int totalQuestions = questions.Count;
            int correctCount = 0;

            foreach (var q in questions)
            {
                var key = "q_" + q.Id;
                if (posted.TryGetValue(key, out var val) && !string.IsNullOrEmpty(val))
                {
                    var selected = val.ToString();
                    _context.TestSubmissions.Add(new TestSubmission
                    {
                        StudentTestId = st.Id,
                        QuestionId = q.Id,
                        SelectedAnswer = selected
                    });

                    if (string.Equals(selected, q.CorrectAnswer, StringComparison.OrdinalIgnoreCase))
                        correctCount++;
                }
            }

            // compute proportional score
            int score = 0;
            if (totalQuestions > 0) score = (int)Math.Round((double)st.Test.FullMarks * correctCount / totalQuestions);

            st.Score = score;
            st.Status = "Submitted";
            st.SubmittedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return RedirectToAction("MyTests");
        }
    }
}
