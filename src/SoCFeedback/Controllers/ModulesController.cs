using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SoCFeedback.Models;
using SoCFeedback.Services;

namespace SoCFeedback.Controllers
{
    public class ModulesController : Controller
    {
        private readonly FeedbackDbContext _context;

        public ModulesController(FeedbackDbContext context)
        {
            _context = context;    
        }

        // GET: Modules
        public async Task<IActionResult> Index()
        {
            var feedbackDbContext = _context.Module.Include(l => l.Level).Include(s => s.Supervisor);
            return View(await feedbackDbContext.ToListAsync());
        }

        // GET: Modules/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @module = await _context.Module
                .Include(l => l.Level)
                .Include(s => s.Supervisor)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (@module == null)
            {
                return NotFound();
            }
            return View(@module);
        }

        // GET: Modules/Create
        public IActionResult Create()
        {
            ViewData["LevelId"] = new SelectList(_context.Level, "Id", "Title");
            ViewData["SupervisorId"] = new SelectList(_context.Supervisor, "Id", "FullName");
            return View();
        }

        // POST: Modules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,Title,Url,LevelId,SupervisorId,Description,Status")] Module @module)
        {
            var obj = _context.Module.Any(e => e.Code.Equals(@module.Code, StringComparison.OrdinalIgnoreCase));
            if (obj)
            {
                ModelState.AddModelError("Code", String.Format("Module with code {0} already exists.", @module.Code));
            }

            if (ModelState.IsValid)
            {
                @module.Id = Guid.NewGuid();
                _context.Add(@module);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["LevelId"] = new SelectList(_context.Level, "Id", "Title", @module.LevelId);
            ViewData["SupervisorId"] = new SelectList(_context.Supervisor, "Id", "FullName", @module.SupervisorId);
            return View(@module);
        }

        // GET: Modules/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @module = await _context.Module.SingleOrDefaultAsync(m => m.Id == id);
            if (@module == null)
            {
                return NotFound();
            }
            ViewData["LevelId"] = new SelectList(_context.Level, "Id", "Title", @module.LevelId);
            ViewData["SupervisorId"] = new SelectList(_context.Supervisor, "Id", "FullName", @module.SupervisorId);
            return View(@module);
        }

        // POST: Modules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Code,Title,Url,LevelId,SupervisorId,Description,Status")] Module @module)
        {
            if (id != @module.Id)
            {
                return NotFound();
            }

            var obj = _context.Module.Any(e => e.Code.Equals(@module.Code, StringComparison.OrdinalIgnoreCase) && e.Id != @module.Id);
            if (obj)
            {
                ModelState.AddModelError("Code", String.Format("Module with code {0} already exists.", @module.Code));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@module);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModuleExists(@module.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["LevelId"] = new SelectList(_context.Level, "Id", "Title", @module.LevelId);
            ViewData["SupervisorId"] = new SelectList(_context.Supervisor, "Id", "FullName", @module.SupervisorId);
            return View(@module);
        }

        // GET: Modules/Archive/5
        public async Task<IActionResult> Archive(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @module = await _context.Module
                .Include(l => l.Level)
                .Include(s => s.Supervisor)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (@module == null)
            {
                return NotFound();
            }

            return View(@module);
        }

        // POST: Modules/Archive/5
        [HttpPost, ActionName("Archive")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ArchiveConfirmed(Guid id)
        {
            var @module = await _context.Module.SingleOrDefaultAsync(m => m.Id == id);
            @module.Status = Enums.Status.Archived;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Modules/Restore/5
        public async Task<IActionResult> Restore(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @module = await _context.Module
                .Include(l => l.Level)
                .Include(s => s.Supervisor)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (@module == null)
            {
                return NotFound();
            }

            return View(@module);
        }

        // POST: Modules/Archive/5
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreConfirmed(Guid id)
        {
            var @module = await _context.Module.SingleOrDefaultAsync(m => m.Id == id);
            @module.Status = Enums.Status.Active;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ModuleExists(Guid id)
        {
            return _context.Module.Any(e => e.Id == id);
        }
    }
}
