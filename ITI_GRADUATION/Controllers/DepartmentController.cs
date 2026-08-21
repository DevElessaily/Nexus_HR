using ITI_GRADUATION.Data;
using ITI_GRADUATION.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITI_GRADUATION.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly AppDbContext _context;
        public DepartmentController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Department/Index
        public async Task<IActionResult> Index()
        {
            var departments = await _context.Departments
                .Include(d => d.Employees)
                .OrderBy(d => d.Name)
                .ToListAsync();
            return View(departments);
        }

        // GET: /Department/Details/1
        // Shows the department along with the list of employees who belong to it.
        public async Task<IActionResult> Details(int id)
        {
            var department = await _context.Departments
                .Include(d => d.Employees)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (department == null)
                return NotFound();

            return View(department);
        }

        // GET: /Department/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Department/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department department)
        {
            if (!ModelState.IsValid)
            {
                return View(department);
            }
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Department/Edit/1
        public async Task<IActionResult> Edit(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        // POST: /Department/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Department dep)
        {
            if (id != dep.Id)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return View(dep);
            }
            _context.Departments.Update(dep);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Department/Delete/1
        public async Task<IActionResult> Delete(int id)
        {
            var department = await _context.Departments
                .Include(d => d.Employees)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        // POST: /Department/Delete/1
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var department = await _context.Departments
                .Include(d => d.Employees)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (department != null)
            {
                if (department.Employees != null && department.Employees.Any())
                {
                    // Prevent deleting a department that still has employees assigned to it.
                    ModelState.AddModelError(string.Empty, "This department still has employees assigned to it and cannot be deleted.");
                    return View("Delete", department);
                }

                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
