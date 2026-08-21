using System.ComponentModel.DataAnnotations;

namespace ITI_GRADUATION.Models
{
    // A simple, standalone entity used to demonstrate full CRUD implemented
    // purely with Razor Pages (see /Pages/Announcements), separate from the
    // MVC controllers used for Employees/Departments/JobTitles.
    public class Announcement
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 150 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Message is required.")]
        [StringLength(2000, MinimumLength = 5, ErrorMessage = "Message must be between 5 and 2000 characters.")]
        public string Message { get; set; } = string.Empty;

        [Display(Name = "Posted On")]
        public DateTime PostedOn { get; set; } = DateTime.Now;
    }
}
