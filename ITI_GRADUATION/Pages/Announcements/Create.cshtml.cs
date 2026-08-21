using ITI_GRADUATION.Data;
using ITI_GRADUATION.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ITI_GRADUATION.Pages.Announcements
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;
        public CreateModel(AppDbContext context) => _context = context;

        [BindProperty]
        public Announcement Announcement { get; set; } = new();

        public void OnGet()
        {
            // Nothing to load - just render an empty form.
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Announcement.PostedOn = DateTime.Now;
            _context.Announcements.Add(Announcement);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
