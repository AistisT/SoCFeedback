using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoCFeedback.Data;
using SoCFeedback.Enums;
using SoCFeedback.Models;

namespace SoCFeedback.Controllers
{
    public class HomeController : Controller
    {
        private readonly FeedbackDbContext _context;

        public HomeController(FeedbackDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var year =
                await _context.Year.Include(y => y.YearModules)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.Status == YearStatus.Published);

            if (year != null)
            {
                year.Modules = YearsController.GetYearModules(year, _context);
                year.Levels = YearsController.GetYearLevels(year);
            }
            return View(year);
        }

        // GET: Module/5
        [Route("Module")]
        public async Task<IActionResult> Module(Guid? id, Guid yid)
        {
            if (id == null)
                return NotFound();
            var year =
                await _context.Year.AsNoTracking()
                    .SingleOrDefaultAsync(y => y.Id == yid && y.Status == YearStatus.Published);
            if (year == null)
                return NotFound();
            var module = await _context.Module.AsNoTracking()
                .Include(s => s.Supervisor)
                .Include(q => q.ModuleQuestions)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (module == null)
                return NotFound();
            module.Questions = _context.Question.Include(c => c.Category)
                .AsNoTracking()
                .OrderBy(q => q.QuestionNumber)
                .ToList();

            for (var i = module.Questions.Count - 1; i >= 0; i--)
            {
                var question = module.Questions[i];
                if (!module.ModuleQuestions.Exists(m => m.QuestionId == question.Id))
                    module.Questions.Remove(question);
            }

            var tempCategory = module.Questions.Select(m => m.Category).ToList();
            module.YearId = yid;
            module.Categories =
                tempCategory.GroupBy(e => e.Title).Select(group => group.First()).OrderBy(e => e.CategoryOrder).ToList();

            return View(module);
        }


        // POST: Module/5
        [Route("Module")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Module(Guid? id, Module module)
        {
            if (id != module.Id)
                return NotFound();

            var dbModule = _context.Module.AsNoTracking().FirstOrDefault(e => e.Id == id);
            var dbYear = _context.Year.AsNoTracking().FirstOrDefault(y => y.Id == module.YearId);
            if (dbModule == null || dbYear == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var question in module.Questions)
                        switch (question.Type)
                        {
                            case QuestionType.Standard:
                                _context.Answer.Add(new Answer
                                {
                                    Answer1 = question.AnswerToSave.Answer1,
                                    ModuleId = module.Id,
                                    QuestionId = question.Id,
                                    Year = dbYear.Year1
                                });
                                break;
                            case QuestionType.Rate:
                                _context.RateAnswer.Add(new RateAnswer
                                {
                                    Rating = question.RateAnswerToSave.Rating,
                                    ModuleId = module.Id,
                                    QuestionId = question.Id,
                                    Year = dbYear.Year1
                                });
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction("Index");
            }

            return View(module);
        }

        [Route("About")]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [Route("Contact")]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [Route("/Error")]
        public IActionResult Error(int error)
        {
            ViewData["Title"] = error;
            ViewData["Error"] = error;
            switch (error)
            {
                case 404:
                    ViewData["Message"] = "Sorry, but this page doesn't exsts.";
                    break;
                case 500:
                    ViewData["Message"] = "Sorry, server has encountered an error";
                    break;
                default:
                    ViewData["Message"] = "An error occurred while processing your request";
                    break;
            }
            return View();
        }
    }
}