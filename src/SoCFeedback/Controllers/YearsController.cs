using System;
using System.Collections.Generic;
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
    [Authorize(Roles = "Admin,Lecturer,LecturerLimited,TeachingStaff")]
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
                return NotFound();

            var year =
                await _context.Year.Include(y => y.YearModules).AsNoTracking().SingleOrDefaultAsync(m => m.Id == id);

            if (year == null)
                return NotFound();

            year.Modules = GetYearModules(year, _context);
            year.Levels = GetYearLevels(year);

            return View(year);
        }

        [Authorize(Roles = "Admin")]
        // GET: Years/Create
        public IActionResult Create()
        {
            var year = new Year
            {
                Modules =
                    _context.Module.Where(m => m.Status == Status.Active && m.Level.Status==Status.Active).OrderBy(o => o.Code).AsNoTracking().ToList(),
                Levels =
                    _context.Level.Where(m => m.Status == Status.Active && m.Module.Count != 0)
                        .OrderBy(o => o.OrderingNumber)
                        .AsNoTracking()
                        .ToList()
            };
            return View(year);
        }

        // POST: Years/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Year1,Status,Modules,Levels")] Year year)
        {
            var obj = _context.Year.Any(e => e.Year1 == year.Year1);
            if (obj)
                ModelState.AddModelError("Year1", $"Academic Year form for year {year.Year1} already exists.");
            if (ModelState.IsValid)
            {
                ArchiveNotArchivedYears();
                var newYear = new Year {Year1 = year.Year1};
                _context.Add(newYear);

                foreach (var module in year.Modules.Where(m => m.RunningStatus == RunningStatus.Active))
                    _context.YearModules.Add(new YearModules
                    {
                        YearId = newYear.Id,
                        ModuleId = module.Id
                    });
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(year);
        }

        private void ArchiveNotArchivedYears()
        {
            var years = _context.Year.Where(y=>y.Status==YearStatus.Published|| y.Status==YearStatus.Pending);
            foreach (var year in years)
            {
                year.Status=YearStatus.Archived;
            }
             _context.SaveChanges();
        }

        // GET: Years/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
                return NotFound();

            var year =
                await _context.Year.AsNoTracking().Include(y => y.YearModules).SingleOrDefaultAsync(m => m.Id == id);
            if (year == null)
                return NotFound();
            if (year.Status == YearStatus.Published)
                RedirectToAction("Index");

            year.Modules =
                _context.Module.AsNoTracking()
                    .Include(l => l.Level)
                    .Where(m => m.Status == Status.Active)
                    .OrderBy(o => o.Code)
                    .ToList();

            var temptList = GetYearModules(year, _context);

            foreach (var module in year.Modules)
                if (!temptList.Exists(m => m.Id == module.Id))
                    module.RunningStatus = RunningStatus.Inactive;

            year.Levels = GetYearLevels(year);

            return View(year);
        }

        // POST: Years/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Year1,Status,Modules,Levels")] Year year)
        {
            if (id != year.Id)
                return NotFound();
            if (year.Status == YearStatus.Published)
                RedirectToAction("Index");
            var obj = _context.Year.AsNoTracking().Any(e => e.Year1 == year.Year1 && e.Id != year.Id);

            if (obj)
                ModelState.AddModelError("Year1", $"Academic year form for year {year.Year1} already exists.");

            if (!ModelState.IsValid) return View(year);

            try
            {
                var dbYear = await _context.Year.Include(y => y.YearModules).SingleOrDefaultAsync(m => m.Id == id);
                dbYear.Year1 = year.Year1;
                foreach (var module in year.Modules)
                {
                    var tYearModule = new YearModules
                    {
                        YearId = year.Id,
                        ModuleId = module.Id
                    };

                    if (!YearModuleExists(tYearModule) && module.RunningStatus == RunningStatus.Active)
                    {
                        dbYear.YearModules.Add(tYearModule);
                        _context.YearModules.Add(tYearModule);
                    }

                    else if (module.RunningStatus == RunningStatus.Inactive)
                    {
                        var moduleToRemove =
                            dbYear.YearModules.SingleOrDefault(y => y.YearId == year.Id && y.ModuleId == module.Id);
                        if (moduleToRemove != null)
                        {
                            dbYear.YearModules.Remove(moduleToRemove);
                            _context.YearModules.Remove(moduleToRemove);
                        }
                    }
                }
                _context.Update(dbYear);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!YearExists(year.Id))
                    return NotFound();
                throw;
            }
            return RedirectToAction("Index");
        }

        // GET: Years/Publish/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Publish(Guid? id)
        {
            if (id == null)
                return NotFound();

            var year =
                await _context.Year.Include(y => y.YearModules).AsNoTracking().SingleOrDefaultAsync(m => m.Id == id);
            if (year == null)
                return NotFound();
            year.Modules = GetYearModules(year, _context);
            year.Levels = GetYearLevels(year);
            return View(year);
        }

        // POST: Years/Publish/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ActionName("Publish")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PublishConfirmed(Guid id)
        {
            // can be remove for final version
            var years = _context.Year.Where(y => y.Status == YearStatus.Published);
            foreach (var dbYear in years)
                dbYear.Status = YearStatus.Archived;

            var year = await _context.Year.SingleOrDefaultAsync(m => m.Id == id);
            year.Status = YearStatus.Published;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Years/Archive/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Archive(Guid? id)
        {
            if (id == null)
                return NotFound();

            var year =
                await _context.Year.Include(y => y.YearModules).AsNoTracking().SingleOrDefaultAsync(m => m.Id == id);
            if (year == null)
                return NotFound();
            year.Modules = GetYearModules(year, _context);
            year.Levels = GetYearLevels(year);
            return View(year);
        }

        // POST: Years/Archive/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ActionName("Archive")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ArchiveConfirmed(Guid id)
        {
            var year = await _context.Year.SingleOrDefaultAsync(m => m.Id == id);
            year.Status = YearStatus.Archived;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool YearExists(Guid id)
        {
            return _context.Year.Any(e => e.Id == id);
        }

        private bool YearModuleExists(YearModules yearModule)
        {
            return
                _context.YearModules.AsNoTracking()
                    .FirstOrDefault(y => y.YearId == yearModule.YearId && y.ModuleId == yearModule.ModuleId) != null;
        }

        public static List<Module> GetYearModules(Year year, FeedbackDbContext context)
        {
            var tempModuleList = new List<Module>();

            foreach (var yearModule in year.YearModules)
                tempModuleList.Add(
                    context.Module.Include(l => l.Level)
                        .AsNoTracking()
                        .SingleOrDefault(m => m.Id == yearModule.ModuleId));
            context.Dispose();
            return tempModuleList.OrderBy(o => o.Code).ToList();
        }

        public static List<Level> GetYearLevels(Year year)
        {
            var tempLevelsList = year.Modules.Select(module => module.Level).ToList();
            return
                tempLevelsList.GroupBy(e => e.Title)
                    .Select(group => group.First())
                    .OrderBy(e => e.OrderingNumber)
                    .ToList();
        }
    }
}