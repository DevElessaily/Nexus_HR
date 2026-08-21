namespace ITI_GRADUATION.Services
{
    public interface IEmailService
    {
        // Fire-and-forget style: implementations should not throw on failure,
        // so a broken mail server never blocks adding an employee.
        Task SendNewEmployeeNotificationAsync(string employeeFullName, string employeeEmail, string? departmentName, string? jobTitleName);
    }
}
