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
    [Authorize(Roles = "Admin,Lecturer,LecturerLimited")]
    public class CategoriesController : Controller
    {
        private readonly FeedbackDbContext _context;

        public CategoriesController(FeedbackDbContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Category.ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
                return NotFound();

            var category = await _context.Category
                .SingleOrDefaultAsync(m => m.Id == id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Status,CategoryOrder")] Category category)
        {
            var obj = _context.Category.Any(e => e.Title.Equals(category.Title, StringComparison.OrdinalIgnoreCase));
            if (obj)
                ModelState.AddModelError("Title", string.Format("Category {0} already exists.", category.Title));

            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
                return NotFound();

            var category = await _context.Category.SingleOrDefaultAsync(m => m.Id == id);
            if (category == null)
                return NotFound();
            return View(category);
        }

        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid? id,
            [Bind("Id,Title,Description,Status,CategoryOrder")] Category category)
        {
            if (id == null)
                return NotFound();

            var obj =
                await _context.Category.SingleOrDefaultAsync(
                    e => e.Title.Equals(category.Title, StringComparison.OrdinalIgnoreCase) && e.Id != category.Id);
            if (obj != null)
                ModelState.AddModelError("Title", string.Format("Category {0} already exists.", category.Title));

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Categories/Archive/5
        public async Task<IActionResult> Archive(Guid? id)
        {
            if (id == null)
                return NotFound();

            var category = await _context.Category
                .SingleOrDefaultAsync(m => m.Id == id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        // POST: Categories/Archive/5
        [HttpPost]
        [ActionName("Archive")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ArchiveConfirmed(Guid id)
        {
            var category = await _context.Category.SingleOrDefaultAsync(m => m.Id == id);
            category.Status = Status.Archived;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Categories/Restore/5
        public async Task<IActionResult> Restore(Guid? id)
        {
            if (id == null)
                return NotFound();

            var category = await _context.Category
                .SingleOrDefaultAsync(m => m.Id == id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        // POST: Categories/Restore/5
        [HttpPost]
        [ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreConfirmed(Guid id)
        {
            var category = await _context.Category.SingleOrDefaultAsync(m => m.Id == id);
            category.Status = Status.Active;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool CategoryExists(Guid id)
        {
            return _context.Category.Any(e => e.Id == id);
        }
    }
}