using System.Diagnostics;
using ITI_GRADUATION.Data;
using ITI_GRADUATION.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITI_GRADUATION.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new DashboardViewModel
            {
                TotalEmployees = await _context.Employees.CountAsync(),
                TotalDepartments = await _context.Departments.CountAsync(),
                TotalJobTitles = await _context.JobTitles.CountAsync(),
                RecentEmployees = await _context.Employees
                    .Include(e => e.Department)
                    .Include(e => e.JobTitle)
                    .OrderByDescending(e => e.HireDate)
                    .Take(5)
                    .ToListAsync()
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
