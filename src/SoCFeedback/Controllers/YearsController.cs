using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SoCFeedback.Models;
using SoCFeedback.Services;
using SoCFeedback.Enums;
using Microsoft.EntityFrameworkCore.Internal;

namespace SoCFeedback.Controllers
{
    public class YearsController : Controller
    {
        private readonly FeedbackDbContext _context;

        public YearsController(FeedbackDbContext context)
        {
            _context = context;
        }

        // GET: Years
        public async Task<IActionResult> Index()
        {
            return View(await _context.Year.ToListAsync());
        }

        // GET: Years/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var year = await _context.Year.Include(y=>y.YearModules).SingleOrDefaultAsync(m => m.Id == id);
            
            if (year == null)
            {
                return NotFound();
            }

            List<Module> tempModuleList=new List<Module>();
          
            foreach (YearModules yearModule in year.YearModules)
            {
                tempModuleList.Add(_context.Module.Include(l=>l.Level).SingleOrDefault(m => m.Id == yearModule.ModuleId));
            }
            year.Modules= tempModuleList.OrderBy(o=>o.Code).ToList();

            List<Level> tempLevelsList = new List<Level>();
            foreach (var module in year.Modules)
            {
                tempLevelsList.Add(module.Level); 
            }
            year.Levels = tempLevelsList.GroupBy(e => e.Title).Select(group => group.First()).OrderBy(e=>e.OrderingNumber).ToList();
            return View(year);
        }

        // GET: Years/Create
        public IActionResult Create()
        {
            Year year = new Year
            {
                Modules = _context.Module.Where(m => m.Status == Status.Active).OrderBy(o => o.Code).AsNoTracking().ToList(),
                Levels = _context.Level.Where(m => m.Status == Status.Active && m.Module.Count != 0).OrderBy(o => o.OrderingNumber).AsNoTracking().ToList()
            };
            return View(year);
        }

        // POST: Years/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Year1,Status,Modules,Levels")] Year year)
        {
            var obj = _context.Year.Any(e => e.Year1 == year.Year1);
            if (obj)
            {
                ModelState.AddModelError("Year1", $"Questionnaire form for year {year.Year1} already exists.");
            }
            if (ModelState.IsValid)
            {
                Year newYear = new Year { Year1 = year.Year1 };
                _context.Add(newYear);

                foreach (Module module in year.Modules.Where(m => m.RunningStatus == ModuleStatus.Running))
                {
                    _context.YearModules.Add(new YearModules
                    {
                        YearId = newYear.Id,
                        ModuleId = module.Id
                    });
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(year);
        }

        // GET: Years/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var year = await _context.Year.SingleOrDefaultAsync(m => m.Id == id);
            if (year == null)
            {
                return NotFound();
            }
            return View(year);
        }

        // POST: Years/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Year1,Status")] Year year)
        {
            if (id != year.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(year);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!YearExists(year.Id))
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
            return View(year);
        }

        // GET: Years/Delete/5
        public async Task<IActionResult> Publish(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var year = await _context.Year
                .SingleOrDefaultAsync(m => m.Id == id);
            if (year == null)
            {
                return NotFound();
            }

            return View(year);
        }

        // POST: Years/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PublishConfirmed(Guid id)
        {
            var year = await _context.Year.SingleOrDefaultAsync(m => m.Id == id);
            year.Status = YearStatus.Published;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool YearExists(Guid id)
        {
            return _context.Year.Any(e => e.Id == id);
        }
    }
}
