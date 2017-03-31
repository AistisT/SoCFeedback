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
using SoCFeedback.Models.ViewModels;

namespace SoCFeedback.Controllers
{
    [Authorize(Roles = "Admin,Lecturer,LecturerLimited,TeachingStaff")]
    public class ModulesController : Controller
    {
        private readonly FeedbackDbContext _context;

        public ModulesController(FeedbackDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admin,Lecturer,LecturerLimited")]
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
                return NotFound();

            var module = await _context.Module
                .Include(l => l.Level)
                .Include(s => s.Supervisor)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (module == null)
                return NotFound();
            return View(module);
        }

        // GET: Modules/Create
        [Authorize(Roles = "Admin,Lecturer,LecturerLimited")]
        public IActionResult Create()
        {
            ViewData["LevelId"] = new SelectList(_context.Level.OrderBy(o => o.OrderingNumber).AsNoTracking().Where(s => s.Status == Status.Active), "Id", "Title");
            ViewData["SupervisorId"] = new SelectList(_context.Supervisor.OrderBy(o => o.Forename).AsNoTracking().Where(s => s.Status == Status.Active), "Id", "FullName");
            return View(new Module());
        }

        // POST: Modules/Create
        [Authorize(Roles = "Admin,Lecturer,LecturerLimited")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,Code,Title,Url,LevelId,SupervisorId,Description,Status")] Module module)
        {
            var obj = _context.Module.Any(e => e.Code.Equals(module.Code, StringComparison.OrdinalIgnoreCase));
            if (obj)
                ModelState.AddModelError("Code", string.Format("Module with code {0} already exists.", module.Code));

            if (ModelState.IsValid)
            {
                module.Id = Guid.NewGuid();
                _context.Add(module);
                await AddQuestions(module);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["LevelId"] = new SelectList(_context.Level.OrderBy(o => o.OrderingNumber).AsNoTracking().Where(s => s.Status == Status.Active), "Id", "Title",
                module.LevelId);
            ViewData["SupervisorId"] = new SelectList(_context.Supervisor.OrderBy(o => o.Forename).AsNoTracking().Where(s => s.Status == Status.Active), "Id", "FullName",
                module.SupervisorId);
            return View(module);
        }

        // GET: Modules/Edit/5
        [Authorize(Roles = "Admin,Lecturer,LecturerLimited")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
                return NotFound();

            var module = await _context.Module.SingleOrDefaultAsync(m => m.Id == id);
            if (module == null)
                return NotFound();
            ViewData["LevelId"] = new SelectList(_context.Level.OrderBy(o => o.OrderingNumber).AsNoTracking()
                .Where(s => s.Status == Status.Active || s.Id == module.LevelId), "Id", "Title",
                module.LevelId);
            ViewData["SupervisorId"] = new SelectList(_context.Supervisor.OrderBy(o => o.Forename).AsNoTracking()
                .Where(s => s.Status == Status.Active || s.Id == module.SupervisorId), "Id", "FullName",
                module.SupervisorId);
            return View(module);
        }

        // POST: Modules/Edit/5
        [Authorize(Roles = "Admin,Lecturer,LecturerLimited")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind("Id,Code,Title,Url,LevelId,SupervisorId,Description,Status")] Module module)
        {
            if (id != module.Id)
                return NotFound();

            var obj =
                _context.Module.Any(
                    e => e.Code.Equals(module.Code, StringComparison.OrdinalIgnoreCase) && e.Id != module.Id);
            if (obj)
                ModelState.AddModelError("Code", string.Format("Module with code {0} already exists.", module.Code));

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(module);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModuleExists(module.Id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction("Index");
            }
            ViewData["LevelId"] = new SelectList(_context.Level.OrderBy(o => o.OrderingNumber).AsNoTracking()
                 .Where(s => s.Status == Status.Active || s.Id == module.LevelId), "Id", "Title",
                 module.LevelId);
            ViewData["SupervisorId"] = new SelectList(_context.Supervisor.OrderBy(o => o.Forename).AsNoTracking()
                .Where(s => s.Status == Status.Active || s.Id == module.SupervisorId), "Id", "FullName",
                module.SupervisorId);
            return View(module);
        }

        // GET: Modules/Archive/5
        [Authorize(Roles = "Admin,Lecturer,LecturerLimited")]
        public async Task<IActionResult> Archive(Guid? id)
        {
            if (id == null)
                return NotFound();

            var module = await _context.Module
                .Include(l => l.Level)
                .Include(s => s.Supervisor)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (module == null)
                return NotFound();

            return View(module);
        }

        // POST: Modules/Archive/5
        [Authorize(Roles = "Admin,Lecturer")]
        [HttpPost]
        [ActionName("Archive")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ArchiveConfirmed(Guid id)
        {
            var module = await _context.Module.SingleOrDefaultAsync(m => m.Id == id);
            module.Status = Status.Archived;

            // remove module that is being archived from pending academic year forms
            var yearModules = _context.YearModules.Where(m => m.ModuleId == module.Id && m.YearNavigation.Status == YearStatus.Pending);
            if (yearModules != null)
            {
                foreach (var ymodule in yearModules)
                {
                    _context.YearModules.Remove(ymodule);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Modules/Restore/5
        [Authorize(Roles = "Admin,Lecturer")]
        public async Task<IActionResult> Restore(Guid? id)
        {
            if (id == null)
                return NotFound();

            var module = await _context.Module
                .Include(l => l.Level)
                .Include(s => s.Supervisor)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (module == null)
                return NotFound();

            return View(module);
        }

        // POST: Modules/Restore/5
        [Authorize(Roles = "Admin,Lecturer")]
        [HttpPost]
        [ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreConfirmed(Guid id)
        {
            var module = await _context.Module.SingleOrDefaultAsync(m => m.Id == id);
            module.Status = Status.Active;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Modules/Questions/5
        [Authorize(Roles = "Admin,Lecturer,LecturerLimited")]
        public async Task<IActionResult> Questions(Guid? id, Guid yid)
        {
            if (id == null)
                return NotFound();
            var module = await _context.Module.AsNoTracking()
                .Include(l => l.Level)
                .Include(s => s.Supervisor)
                .Include(q => q.ModuleQuestions)
                .SingleOrDefaultAsync(m => m.Id == id);

            var year = await _context.Year.AsNoTracking().SingleOrDefaultAsync(y => y.Id == yid);
            if (module == null || year == null)
                return NotFound();

            if (year.Status != YearStatus.Pending)
            {
                return NotFound();
            }

            module.Questions = _context.Question.Include(c => c.Category)
                .AsNoTracking()
                .Where(q => q.Status == Status.Active && q.Category.Status ==Status.Active)
                .OrderBy(q => q.QuestionNumber)
                .ToList();

            foreach (var question in module.Questions)
                if (!module.ModuleQuestions.Exists(m => m.QuestionId == question.Id))
                    question.RunningStatus = RunningStatus.Inactive;

            module.YearId = yid;
            var tempCategory = module.Questions.Select(m => m.Category).ToList();
            module.Categories =
                tempCategory.GroupBy(e => e.Title).Select(group => group.First()).OrderBy(e => e.CategoryOrder).ToList();


            return View(module);
        }

        // POST: Modules/Questions/5
        [Authorize(Roles = "Admin,Lecturer,LecturerLimited")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Questions(Guid? id, [Bind("Id,Questions,Categories,YearId")] Module module)
        {
            var dbYear = _context.Year.AsNoTracking().SingleOrDefault(y => y.Id == module.YearId);
            if (id == null || dbYear == null)
                return NotFound();
            if (dbYear.Status != YearStatus.Published)
                try
                {
                    var dbModule =
                        await _context.Module.Include(y => y.ModuleQuestions).SingleOrDefaultAsync(m => m.Id == id);
                    foreach (var question in module.Questions)
                    {
                        var tModuleQuestion = new ModuleQuestions
                        {
                            ModuleId = module.Id,
                            QuestionId = question.Id
                        };

                        if (!ModuleQuestionExists(tModuleQuestion) && question.RunningStatus == RunningStatus.Active)
                        {
                            dbModule.ModuleQuestions.Add(tModuleQuestion);
                            _context.ModuleQuestions.Add(tModuleQuestion);
                        }

                        else if (question.RunningStatus == RunningStatus.Inactive)
                        {
                            var questionToRemove =
                                dbModule.ModuleQuestions.SingleOrDefault(
                                    y => y.ModuleId == module.Id && y.QuestionId == question.Id);
                            if (questionToRemove != null)
                            {
                                dbModule.ModuleQuestions.Remove(questionToRemove);
                                _context.ModuleQuestions.Remove(questionToRemove);
                            }
                        }
                    }
                    _context.Update(dbModule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModuleExists(module.Id))
                        return NotFound();
                    throw;
                }
            return RedirectToAction("Details", "Years", new { id = module.YearId });
        }

        // GET: Modules/Feedback/5
        public async Task<IActionResult> Feedback(Guid? id, Guid yid)
        {
            if (id == null)
                return NotFound();
            var module = await _context.Module.AsNoTracking()
                .Include(l => l.Level)
                .Include(s => s.Supervisor)
                .SingleOrDefaultAsync(m => m.Id == id);

            var year = await _context.Year.AsNoTracking().SingleOrDefaultAsync(y => y.Id == yid);
            if (module == null || year == null)
                return NotFound();
            if (year.Status == YearStatus.Pending)
            {
                return NotFound();
            }
            var viewModel = new FeedbackViewModel
            {
                Code = module.Code,
                Level = module.Level.Title,
                Title = module.Title,
                Coordinator = $"{module.Supervisor.Title} {module.Supervisor.Forename} {module.Supervisor.Surname}",
                YearId = year.Id
            };

            var answers = _context.Answer
                .Include(a => a.Question)
                .Include(b => b.Question.Category)
                .AsNoTracking()
                .Where(m => m.ModuleId == id && m.Year == year.Year1).ToList();
            foreach (var answer in answers)
                if (!viewModel.Questions.Exists(q => q.Id == answer.QuestionId))
                {
                    var temp = new QuestionViewModel
                    {
                        Type = answer.Question.Type,
                        Question = answer.Question.Question1,
                        Category = new CategoriesViewModel
                        {
                            CategoryId = answer.Question.CategoryId,
                            Title = answer.Question.Category.Title,
                            Order = answer.Question.Category.CategoryOrder
                        },
                        Order = answer.Question.QuestionNumber,
                        Id = answer.QuestionId
                    };
                    temp.StandardAnswers.Add(answer.Answer1);
                    viewModel.Questions.Add(temp);
                }
                else
                {
                    viewModel.Questions.Find(q => q.Id == answer.QuestionId).StandardAnswers.Add(answer.Answer1);
                }

            var rateAnswers = _context.RateAnswer
                .Include(a => a.Question)
                .Include(b => b.Question.Category)
                .AsNoTracking()
                .Where(m => m.ModuleId == id && m.Year == year.Year1);
            foreach (var answer in rateAnswers)
                if (!viewModel.Questions.Exists(q => q.Id == answer.QuestionId))
                {
                    var temp = new QuestionViewModel
                    {
                        Type = answer.Question.Type,
                        Question = answer.Question.Question1,
                        Category = new CategoriesViewModel
                        {
                            CategoryId = answer.Question.CategoryId,
                            Title = answer.Question.Category.Title,
                            Order = answer.Question.Category.CategoryOrder
                        },
                        Order = answer.Question.QuestionNumber,
                        Id = answer.QuestionId
                    };
                    temp.RateAnswers.Add(answer.Rating);
                    viewModel.Questions.Add(temp);
                }
                else
                {
                    viewModel.Questions.Find(q => q.Id == answer.QuestionId).RateAnswers.Add(answer.Rating);
                }

            viewModel.Questions = viewModel.Questions.OrderBy(q => q.Order).ToList();

            foreach (var question in viewModel.Questions)
            {
                viewModel.Categories.Add(question.Category);
                question.SetLabelsList();
                question.SetRatingsList();
            }

            viewModel.Categories = viewModel.Categories.GroupBy(e => e.Title)
                .Select(group => group.First())
                .OrderBy(e => e.Order)
                .ToList();
            
            viewModel.SetAverage();
            viewModel.SetLabelsList();
            viewModel.SetRatingsList();
            return View(viewModel);
        }

        private bool ModuleExists(Guid id)
        {
            return _context.Module.Any(e => e.Id == id);
        }

        private async Task AddQuestions(Module module)
        {
            var questionsList =
                _context.Question.AsNoTracking().Where(q => q.Status == Status.Active && q.Optional == false).ToList();

            foreach (var question in questionsList)
                _context.ModuleQuestions.Add(new ModuleQuestions
                {
                    QuestionId = question.Id,
                    ModuleId = module.Id
                });
            await _context.SaveChangesAsync();
        }

        private bool ModuleQuestionExists(ModuleQuestions moduleQuestion)
        {
            return
                _context.ModuleQuestions.AsNoTracking()
                    .FirstOrDefault(
                        y => y.QuestionId == moduleQuestion.QuestionId && y.ModuleId == moduleQuestion.ModuleId) != null;
        }
    }
}