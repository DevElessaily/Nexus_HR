using ITI_GRADUATION.Data;
using ITI_GRADUATION.Models;
using ITI_GRADUATION.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ITI_GRADUATION.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly IEmailService _emailService;
        private const int PageSize = 5;

        private static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
        private const long MaxImageSizeBytes = 2 * 1024 * 1024; // 2 MB

        public EmployeeController(AppDbContext context, IWebHostEnvironment environment, IEmailService emailService)
        {
            _context = context;
            _environment = environment;
            _emailService = emailService;
        }

        // GET: /Employee/Index
        // Supports free-text search (by name or email), filtering by department/job title, and pagination.
        public async Task<IActionResult> Index(string? searchString, int? departmentId, int? jobTitleId, int pageNumber = 1)
        {
            var query = _context.Employees
                .Include(e => e.Department)
                .Include(e => e.JobTitle)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                var term = searchString.Trim();
                query = query.Where(e => e.FullName.Contains(term) || e.Email.Contains(term));
            }

            if (departmentId.HasValue)
            {
                query = query.Where(e => e.DepartmentId == departmentId.Value);
            }

            if (jobTitleId.HasValue)
            {
                query = query.Where(e => e.JobTitleId == jobTitleId.Value);
            }

            query = query.OrderBy(e => e.FullName);

            var paginatedEmployees = await PaginatedList<Employee>.CreateAsync(query, pageNumber < 1 ? 1 : pageNumber, PageSize);

            var viewModel = new EmployeeIndexViewModel
            {
                Employees = paginatedEmployees,
                SearchString = searchString,
                DepartmentId = departmentId,
                JobTitleId = jobTitleId,
                Departments = await _context.Departments.OrderBy(d => d.Name).ToListAsync(),
                JobTitles = await _context.JobTitles.OrderBy(j => j.Title).ToListAsync()
            };

            return View(viewModel);
        }

        // GET: /Employee/Details/1
        public async Task<IActionResult> Details(int id)
        {
            var employee = await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.JobTitle)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
                return NotFound();

            return View(employee);
        }

        // GET: /Employee/Create
        public async Task<IActionResult> Create()
        {
            await PopulateDropdownsAsync();
            return View();
        }

        // POST: /Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync(employee.DepartmentId, employee.JobTitleId);
                return View(employee);
            }

            employee.ProfileImagePath = await SaveProfileImageAsync(employee.ProfileImageFile);

            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync(employee.DepartmentId, employee.JobTitleId);
                return View(employee);
            }

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            // Notify by email that a new employee was added. Never blocks or fails the request -
            // SmtpEmailService swallows its own errors (e.g. missing/invalid SMTP settings).
            var department = await _context.Departments.FindAsync(employee.DepartmentId);
            var jobTitle = await _context.JobTitles.FindAsync(employee.JobTitleId);
            await _emailService.SendNewEmployeeNotificationAsync(employee.FullName, employee.Email, department?.Name, jobTitle?.Title);

            return RedirectToAction(nameof(Index));
        }

        // GET: /Employee/Edit/1
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
                return NotFound();

            await PopulateDropdownsAsync(employee.DepartmentId, employee.JobTitleId);
            return View(employee);
        }

        // POST: /Employee/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
            if (id != employee.Id)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync(employee.DepartmentId, employee.JobTitleId);
                return View(employee);
            }

            var employeeInDb = await _context.Employees.FindAsync(id);
            if (employeeInDb == null)
                return NotFound();

            employeeInDb.FullName = employee.FullName;
            employeeInDb.Email = employee.Email;
            employeeInDb.PhoneNumber = employee.PhoneNumber;
            employeeInDb.HireDate = employee.HireDate;
            employeeInDb.Salary = employee.Salary;
            employeeInDb.DepartmentId = employee.DepartmentId;
            employeeInDb.JobTitleId = employee.JobTitleId;

            if (employee.ProfileImageFile != null)
            {
                var savedPath = await SaveProfileImageAsync(employee.ProfileImageFile);
                if (!ModelState.IsValid)
                {
                    await PopulateDropdownsAsync(employee.DepartmentId, employee.JobTitleId);
                    return View(employee);
                }

                DeleteProfileImage(employeeInDb.ProfileImagePath);
                employeeInDb.ProfileImagePath = savedPath;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Employee/Delete/1
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.JobTitle)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
                return NotFound();

            return View(employee);
        }

        // POST: /Employee/Delete/1
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                DeleteProfileImage(employee.ProfileImagePath);
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateDropdownsAsync(int? selectedDepartmentId = null, int? selectedJobTitleId = null)
        {
            ViewBag.DepartmentId = new SelectList(await _context.Departments.OrderBy(d => d.Name).ToListAsync(), "Id", "Name", selectedDepartmentId);
            ViewBag.JobTitleId = new SelectList(await _context.JobTitles.OrderBy(j => j.Title).ToListAsync(), "Id", "Title", selectedJobTitleId);
        }

        // Validates and saves an uploaded profile image under wwwroot/uploads/employees,
        // returning the relative path to store on the Employee record (or null if no file was provided).
        private async Task<string?> SaveProfileImageAsync(IFormFile? file)
        {
            if (file == null || file.Length == 0)
                return null;

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedImageExtensions.Contains(extension))
            {
                ModelState.AddModelError("ProfileImageFile", "Only .jpg, .jpeg, .png and .gif images are allowed.");
                return null;
            }

            if (file.Length > MaxImageSizeBytes)
            {
                ModelState.AddModelError("ProfileImageFile", "Image size must not exceed 2 MB.");
                return null;
            }

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "employees");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/employees/{uniqueFileName}";
        }

        private void DeleteProfileImage(string? relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return;

            var fullPath = Path.Combine(_environment.WebRootPath, relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }
    }
}
