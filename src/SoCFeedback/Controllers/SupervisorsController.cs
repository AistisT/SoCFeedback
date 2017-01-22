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
    public class SupervisorsController : Controller
    {
        private readonly FeedbackDbContext _context;

        public SupervisorsController(FeedbackDbContext context)
        {
            _context = context;
        }

        // GET: Supervisors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Supervisor.ToListAsync());
        }

        // GET: Supervisors/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
                return NotFound();

            var supervisor = await _context.Supervisor
                .SingleOrDefaultAsync(m => m.Id == id);
            if (supervisor == null)
                return NotFound();

            return View(supervisor);
        }

        // GET: Supervisors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Supervisors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,Title,Forename,Surname,Email,PhoneNr,Status")] Supervisor supervisor)
        {
            var obj =
                _context.Supervisor.Any(e => e.Forename.Equals(supervisor.Forename, StringComparison.OrdinalIgnoreCase)
                                             && e.Surname.Equals(supervisor.Surname, StringComparison.OrdinalIgnoreCase));
            if (obj)
            {
                ModelState.AddModelError("Forename",
                    string.Format("Supervisor {0} {1} already exists.", supervisor.Forename, supervisor.Surname));
                ModelState.AddModelError("Surname",
                    string.Format("Supervisor {0} {1} already exists.", supervisor.Forename, supervisor.Surname));
            }

            var emailCheck =
                _context.Supervisor.Any(e => e.Email.Equals(supervisor.Email, StringComparison.OrdinalIgnoreCase));
            if (emailCheck)
                ModelState.AddModelError("Email", string.Format("Email {0} is already in use.", supervisor.Email));

            if (ModelState.IsValid)
            {
                supervisor.Id = Guid.NewGuid();
                _context.Add(supervisor);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(supervisor);
        }

        // GET: Supervisors/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
                return NotFound();

            var supervisor = await _context.Supervisor.SingleOrDefaultAsync(m => m.Id == id);
            if (supervisor == null)
                return NotFound();
            return View(supervisor);
        }

        // POST: Supervisors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind("Id,Title,Forename,Surname,Email,PhoneNr,Status")] Supervisor supervisor)
        {
            if (id != supervisor.Id)
                return NotFound();

            var obj =
                _context.Supervisor.Any(e => e.Forename.Equals(supervisor.Forename, StringComparison.OrdinalIgnoreCase)
                                             && e.Surname.Equals(supervisor.Surname, StringComparison.OrdinalIgnoreCase)
                                             && e.Id != supervisor.Id);
            if (obj)
            {
                ModelState.AddModelError("Forename",
                    string.Format("Supervisor {0} {1} already exists.", supervisor.Forename, supervisor.Surname));
                ModelState.AddModelError("Surname",
                    string.Format("Supervisor {0} {1} already exists.", supervisor.Forename, supervisor.Surname));
            }

            var emailCheck =
                _context.Supervisor.Any(
                    e => e.Email.Equals(supervisor.Email, StringComparison.OrdinalIgnoreCase) && e.Id != supervisor.Id);
            if (emailCheck)
                ModelState.AddModelError("Email", string.Format("Email {0} is already in use.", supervisor.Email));

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(supervisor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupervisorExists(supervisor.Id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction("Index");
            }
            return View(supervisor);
        }

        // GET: Supervisors/Archive/5
        public async Task<IActionResult> Archive(Guid? id)
        {
            if (id == null)
                return NotFound();

            var supervisor = await _context.Supervisor
                .SingleOrDefaultAsync(m => m.Id == id);
            if (supervisor == null)
                return NotFound();

            return View(supervisor);
        }

        // POST: Supervisors/Archive/5
        [HttpPost]
        [ActionName("Archive")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ArchiveConfirmed(Guid id)
        {
            var supervisor = await _context.Supervisor.SingleOrDefaultAsync(m => m.Id == id);
            supervisor.Status = Status.Archived;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Supervisors/Restore/5
        public async Task<IActionResult> Restore(Guid? id)
        {
            if (id == null)
                return NotFound();

            var supervisor = await _context.Supervisor
                .SingleOrDefaultAsync(m => m.Id == id);
            if (supervisor == null)
                return NotFound();

            return View(supervisor);
        }

        // POST: Supervisors/Archive/5
        [HttpPost]
        [ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreConfirmed(Guid id)
        {
            var supervisor = await _context.Supervisor.SingleOrDefaultAsync(m => m.Id == id);
            supervisor.Status = Status.Active;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool SupervisorExists(Guid id)
        {
            return _context.Supervisor.Any(e => e.Id == id);
        }
    }
}