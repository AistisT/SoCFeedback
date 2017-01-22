using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoCFeedback.Data;
using SoCFeedback.Enums;
using SoCFeedback.Models;

namespace SoCFeedback.Controllers
{
    [Authorize(Roles = "Admin,Lecturer")]
    public class LevelsController : Controller
    {
        private readonly FeedbackDbContext _context;

        public LevelsController(FeedbackDbContext context)
        {
            _context = context;
        }

        // GET: Levels
        public async Task<IActionResult> Index()
        {
            return View(await _context.Level.ToListAsync());
        }

        // GET: Levels/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
                return NotFound();

            var level = await _context.Level
                .SingleOrDefaultAsync(m => m.Id == id);
            if (level == null)
                return NotFound();

            return View(level);
        }

        // GET: Levels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Levels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Status,Description,OrderingNumber")] Level level)
        {
            var obj = _context.Level.Any(e => e.Title.Equals(level.Title, StringComparison.OrdinalIgnoreCase));
            if (obj)
                ModelState.AddModelError("Title", string.Format("Level {0} already exists.", level.Title));

            if (ModelState.IsValid)
            {
                level.Id = Guid.NewGuid();
                _context.Add(level);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(level);
        }

        // GET: Levels/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
                return NotFound();

            var level = await _context.Level.SingleOrDefaultAsync(m => m.Id == id);
            if (level == null)
                return NotFound();
            return View(level);
        }

        // POST: Levels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,Status,Description,OrderingNumber")] Level level)
        {
            if (id != level.Id)
                return NotFound();

            var obj =
                await _context.Level.SingleOrDefaultAsync(
                    e => e.Title.Equals(level.Title, StringComparison.OrdinalIgnoreCase) && e.Id != level.Id);
            if (obj != null)
                ModelState.AddModelError("Title", string.Format("Level {0} already exists.", level.Title));

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(level);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LevelExists(level.Id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction("Index");
            }
            return View(level);
        }

        // GET: Levels/Archive/5
        public async Task<IActionResult> Archive(Guid? id)
        {
            if (id == null)
                return NotFound();

            var level = await _context.Level
                .SingleOrDefaultAsync(m => m.Id == id);
            if (level == null)
                return NotFound();

            return View(level);
        }

        // POST: Levels/Archive/5
        [HttpPost]
        [ActionName("Archive")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ArchiveConfirmed(Guid id)
        {
            var level = await _context.Level.SingleOrDefaultAsync(m => m.Id == id);
            level.Status = Status.Archived;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Categories/Restore/5
        public async Task<IActionResult> Restore(Guid? id)
        {
            if (id == null)
                return NotFound();

            var level = await _context.Level
                .SingleOrDefaultAsync(m => m.Id == id);
            if (level == null)
                return NotFound();

            return View(level);
        }

        // POST: Categories/Restore/5
        [HttpPost]
        [ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreConfirmed(Guid id)
        {
            var level = await _context.Level.SingleOrDefaultAsync(m => m.Id == id);
            level.Status = Status.Active;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool LevelExists(Guid id)
        {
            return _context.Level.Any(e => e.Id == id);
        }
    }
}