namespace ITI_GRADUATION.Models
{
    public class DashboardViewModel
    {
        public int TotalEmployees { get; set; }
        public int TotalDepartments { get; set; }
        public int TotalJobTitles { get; set; }
        public List<Employee> RecentEmployees { get; set; } = new();
    }
}
