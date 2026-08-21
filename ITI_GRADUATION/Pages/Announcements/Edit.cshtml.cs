using ITI_GRADUATION.Data;
using ITI_GRADUATION.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ITI_GRADUATION.Pages.Announcements
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;
        public EditModel(AppDbContext context) => _context = context;

        [BindProperty]
        public Announcement Announcement { get; set; } = new();

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
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var announcementInDb = await _context.Announcements.FindAsync(Announcement.Id);
            if (announcementInDb == null)
                return NotFound();

            announcementInDb.Title = Announcement.Title;
            announcementInDb.Message = Announcement.Message;
            // PostedOn is intentionally left untouched on edit - it reflects the original post date.

            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
