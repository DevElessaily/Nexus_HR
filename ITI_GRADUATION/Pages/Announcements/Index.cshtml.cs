using ITI_GRADUATION.Data;
using ITI_GRADUATION.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ITI_GRADUATION.Pages.Announcements
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;
        public IndexModel(AppDbContext context) => _context = context;

        public IList<Announcement> Announcements { get; set; } = new List<Announcement>();

        public async Task OnGetAsync()
        {
            Announcements = await _context.Announcements
                .OrderByDescending(a => a.PostedOn)
                .ToListAsync();
        }
    }
}
