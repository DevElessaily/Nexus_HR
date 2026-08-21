using ITI_GRADUATION.Data;
using ITI_GRADUATION.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ITI_GRADUATION.Pages.Announcements
{
    public class DeleteModel : PageModel
    {
        private readonly AppDbContext _context;
        public DeleteModel(AppDbContext context) => _context = context;

        [BindProperty]
        public Announcement Announcement { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var announcement = await _context.Announcements.FindAsync(id);
            if (announcement == null)
                return NotFound();

            Announcement = announcement;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var announcement = await _context.Announcements.FindAsync(Announcement.Id);
            if (announcement != null)
            {
                _context.Announcements.Remove(announcement);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
