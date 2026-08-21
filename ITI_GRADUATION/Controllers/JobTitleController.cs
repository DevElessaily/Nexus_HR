using ITI_GRADUATION.Data;
using ITI_GRADUATION.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITI_GRADUATION.Controllers
{
    // This controller was missing entirely from the original project even though
    // JobTitle is a required foreign key on Employee. Built to match the
    // Department controller's structure for consistency.
    public class JobTitleController : Controller
    {
        private readonly AppDbContext _context;
        public JobTitleController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /JobTitle/Index
        public async Task<IActionResult> Index()
        {
            var jobTitles = await _context.JobTitles
                .Include(j => j.Employees)
                .OrderBy(j => j.Title)
                .ToListAsync();
            return View(jobTitles);
        }

        // GET: /JobTitle/Details/1
        public async Task<IActionResult> Details(int id)
        {
            var jobTitle = await _context.JobTitles
                .Include(j => j.Employees)
                .FirstOrDefaultAsync(j => j.Id == id);

            if (jobTitle == null)
                return NotFound();

            return View(jobTitle);
        }

        // GET: /JobTitle/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /JobTitle/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JobTitle jobTitle)
        {
            if (!ModelState.IsValid)
            {
                return View(jobTitle);
            }
            _context.JobTitles.Add(jobTitle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /JobTitle/Edit/1
        public async Task<IActionResult> Edit(int id)
        {
            var jobTitle = await _context.JobTitles.FindAsync(id);
            if (jobTitle == null)
            {
                return NotFound();
            }
            return View(jobTitle);
        }

        // POST: /JobTitle/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, JobTitle jobTitle)
        {
            if (id != jobTitle.Id)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return View(jobTitle);
            }
            _context.JobTitles.Update(jobTitle);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /JobTitle/Delete/1
        public async Task<IActionResult> Delete(int id)
        {
            var jobTitle = await _context.JobTitles
                .Include(j => j.Employees)
                .FirstOrDefaultAsync(j => j.Id == id);

            if (jobTitle == null)
            {
                return NotFound();
            }
            return View(jobTitle);
        }

        // POST: /JobTitle/Delete/1
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jobTitle = await _context.JobTitles
                .Include(j => j.Employees)
                .FirstOrDefaultAsync(j => j.Id == id);

            if (jobTitle != null)
            {
                if (jobTitle.Employees != null && jobTitle.Employees.Any())
                {
                    ModelState.AddModelError(string.Empty, "This job title still has employees assigned to it and cannot be deleted.");
                    return View("Delete", jobTitle);
                }

                _context.JobTitles.Remove(jobTitle);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
