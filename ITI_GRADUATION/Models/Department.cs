using System.ComponentModel.DataAnnotations;

namespace ITI_GRADUATION.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Department name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Department name must be between 2 and 100 characters.")]
        [Display(Name = "Department Name")]
        public string Name { get; set; }

        public ICollection<Employee>? Employees { get; set; }
    }
}
