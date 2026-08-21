using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITI_GRADUATION.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Full name must be between 3 and 100 characters.")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Hire date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Salary is required.")]
        [Range(0, 10000000, ErrorMessage = "Salary must be a positive value.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Salary { get; set; }

        // Foreign Key
        [Required(ErrorMessage = "Please select a department.")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        // Navigation Property
        public Department? Department { get; set; }

        // Foreign Key
        [Required(ErrorMessage = "Please select a job title.")]
        [Display(Name = "Job Title")]
        public int JobTitleId { get; set; }

        // Navigation Property
        public JobTitle? JobTitle { get; set; }

        [Display(Name = "Profile Image")]
        public string? ProfileImagePath { get; set; }

        // Not mapped to the database - used only to receive the uploaded file from the form
        [NotMapped]
        [Display(Name = "Profile Image")]
        public IFormFile? ProfileImageFile { get; set; }
    }
}
