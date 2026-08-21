using System.ComponentModel.DataAnnotations;

namespace ITI_GRADUATION.Models
{
    public class JobTitle
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Job title is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Job title must be between 2 and 100 characters.")]
        [Display(Name = "Job Title")]
        public string Title { get; set; }

        public ICollection<Employee>? Employees { get; set; }
    }
}
