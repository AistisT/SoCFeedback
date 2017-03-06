using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SoCFeedback.Data;
using SoCFeedback.Enums;
using SoCFeedback.Models;

namespace SoCFeedback.Controllers
{
    [Authorize(Roles = "Admin,Lecturer,LecturerLimited")]
    public class QuestionsController : Controller
    {
        private readonly FeedbackDbContext _context;

        public QuestionsController(FeedbackDbContext context)
        {
            _context = context;
        }

        // GET: Questions
        public async Task<IActionResult> Index()
        {
            var feedbackDbContext = _context.Question.Include(q => q.Category);
            return View(await feedbackDbContext.ToListAsync());
        }

        // GET: Questions/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
                return NotFound();

            var question = await _context.Question
                .Include(q => q.Category)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (question == null)
                return NotFound();

            return View(question);
        }

        // GET: Questions/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category.AsNoTracking().Where(c => c.Status == Status.Active).OrderBy(c => c.CategoryOrder), "Id", "Title");
            return View(new Question());
        }

        // POST: Questions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Question1,Type,CategoryId,Optional,QuestionNumber")] Question question)
        {
            var obj =
                await _context.Question.AnyAsync(
                    e => e.Question1.Equals(question.Question1, StringComparison.OrdinalIgnoreCase));
            if (obj)
                ModelState.AddModelError("Question1", string.Format("Question {0} already exists.", question.Question1));

            if (ModelState.IsValid)
            {
                question.Id = Guid.NewGuid();
                _context.Add(question);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["CategoryId"] = new SelectList(_context.Category.AsNoTracking().Where(c => c.Status == Status.Active).OrderBy(c => c.CategoryOrder), "Id", "Title", question.CategoryId);
            return View(question);
        }

        // GET: Questions/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
                return NotFound();

            var question = await _context.Question.SingleOrDefaultAsync(m => m.Id == id);
            if (question == null)
                return NotFound();
            ViewData["CategoryId"] = new SelectList(_context.Category
                .AsNoTracking().Where(c => c.Status == Status.Active || c.Id == question.CategoryId).OrderBy(c => c.CategoryOrder)
                , "Id", "Title", question.CategoryId);
            return View(question);
        }

        // POST: Questions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind("Id,Question1,Type,CategoryId,Optional,Status,QuestionNumber")] Question question)
        {
            if (id != question.Id)
                return NotFound();

            var obj =
                await _context.Question.SingleOrDefaultAsync(
                    e =>
                        e.Question1.Equals(question.Question1, StringComparison.OrdinalIgnoreCase) &&
                        e.Id != question.Id);
            if (obj != null)
                ModelState.AddModelError("Question1", string.Format("Question {0} already exists.", question.Question1));

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(question);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(question.Id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction("Index");
            }
            ViewData["CategoryId"] = new SelectList(_context.Category
                .AsNoTracking().Where(c => c.Status == Status.Active || c.Id == question.CategoryId).OrderBy(c => c.CategoryOrder)
                , "Id", "Title", question.CategoryId);
            return View(question);
        }

        // GET: Questions/Archive/5
        public async Task<IActionResult> Archive(Guid? id)
        {
            if (id == null)
                return NotFound();

            var question = await _context.Question
                .Include(q => q.Category)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (question == null)
                return NotFound();

            return View(question);
        }

        // POST: Questions/Archive/5
        [HttpPost]
        [ActionName("Archive")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ArchiveConfirmed(Guid id)
        {
            var question = await _context.Question.SingleOrDefaultAsync(m => m.Id == id);
            question.Status = Status.Archived;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Questions/Restore/5
        public async Task<IActionResult> Restore(Guid? id)
        {
            if (id == null)
                return NotFound();

            var question = await _context.Question
                .Include(q => q.Category)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (question == null)
                return NotFound();

            return View(question);
        }

        // POST: Questions/Restore/5
        [HttpPost]
        [ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreConfirmed(Guid id)
        {
            var question = await _context.Question.SingleOrDefaultAsync(m => m.Id == id);
            question.Status = Status.Active;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool QuestionExists(Guid id)
        {
            return _context.Question.Any(e => e.Id == id);
        }
    }
}