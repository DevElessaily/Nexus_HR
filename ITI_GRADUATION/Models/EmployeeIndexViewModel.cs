namespace ITI_GRADUATION.Models
{
    // Carries everything the Employee/Index view needs: the current page
    // of employees plus the search/filter state, so the view can keep
    // the search box, dropdowns and pagination links in sync.
    public class EmployeeIndexViewModel
    {
        public PaginatedList<Employee> Employees { get; set; } = default!;

        public string? SearchString { get; set; }
        public int? DepartmentId { get; set; }
        public int? JobTitleId { get; set; }

        public IEnumerable<Department> Departments { get; set; } = new List<Department>();
        public IEnumerable<JobTitle> JobTitles { get; set; } = new List<JobTitle>();
    }
}
