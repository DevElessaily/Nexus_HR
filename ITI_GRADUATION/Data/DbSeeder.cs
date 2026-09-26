using ITI_GRADUATION.Models;
using Microsoft.EntityFrameworkCore;

namespace ITI_GRADUATION.Data
{
    /// <summary>
    /// Seeds the database with a realistic starter dataset the first time it runs.
    /// Idempotent: if departments already exist it skips, so it is safe to run on every startup.
    /// </summary>
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext db)
        {
            // Departments -------------------------------------------------------
            if (!await db.Departments.AnyAsync())
            {
                var departments = new[]
                {
                    new Department { Name = "Human Resources" },
                    new Department { Name = "Information Technology" },
                    new Department { Name = "Finance" },
                    new Department { Name = "Marketing" },
                };
                db.Departments.AddRange(departments);
                await db.SaveChangesAsync();
            }

            // Job Titles --------------------------------------------------------
            if (!await db.JobTitles.AnyAsync())
            {
                var jobTitles = new[]
                {
                    new JobTitle { Title = "Software Engineer" },
                    new JobTitle { Title = "HR Specialist" },
                    new JobTitle { Title = "Accountant" },
                    new JobTitle { Title = "Marketing Coordinator" },
                    new JobTitle { Title = "IT Support Specialist" },
                };
                db.JobTitles.AddRange(jobTitles);
                await db.SaveChangesAsync();
            }

            // Ensure lookups are loaded so we can link employees below.
            var departmentsList   = await db.Departments.OrderBy(d => d.Name).ToListAsync();
            var jobTitlesList     = await db.JobTitles.OrderBy(j => j.Title).ToListAsync();

            // Employees ----------------------------------------------------------
            if (!await db.Employees.AnyAsync())
            {
                var employees = new[]
                {
                    new Employee
                    {
                        FullName = "Rawan El Shazly",
                        Email = "rawanelshazly05@gmail.com",
                        PhoneNumber = "01000000001",
                        HireDate = new DateTime(2024, 3, 1),
                        Salary = 15000m,
                        DepartmentId = departmentsList.First(d => d.Name == "Information Technology").Id,
                        JobTitleId = jobTitlesList.First(j => j.Title == "Software Engineer").Id,
                        ProfileImagePath = "/uploads/employees/seed-1.jpg"
                    },
                    new Employee
                    {
                        FullName = "Moahmed Ali Hassan",
                        Email = "mohamed.ali@example.com",
                        PhoneNumber = "01000000002",
                        HireDate = new DateTime(2023, 7, 15),
                        Salary = 12000m,
                        DepartmentId = departmentsList.First(d => d.Name == "Human Resources").Id,
                        JobTitleId = jobTitlesList.First(j => j.Title == "HR Specialist").Id,
                        ProfileImagePath = "/uploads/employees/seed-2.jpg"
                    },
                    new Employee
                    {
                        FullName = "Sara Mahmoud",
                        Email = "sara.mahmoud@example.com",
                        PhoneNumber = "01000000003",
                        HireDate = new DateTime(2024, 6, 10),
                        Salary = 18000m,
                        DepartmentId = departmentsList.First(d => d.Name == "Finance").Id,
                        JobTitleId = jobTitlesList.First(j => j.Title == "Accountant").Id,
                        ProfileImagePath = "/uploads/employees/seed-3.jpg"
                    },
                    new Employee
                    {
                        FullName = "Nour Youssef",
                        Email = "nour.youssef@example.com",
                        PhoneNumber = "01000000004",
                        HireDate = new DateTime(2025, 1, 5),
                        Salary = 11000m,
                        DepartmentId = departmentsList.First(d => d.Name == "Marketing").Id,
                        JobTitleId = jobTitlesList.First(j => j.Title == "Marketing Coordinator").Id,
                        ProfileImagePath = "/uploads/employees/seed-4.jpg"
                    },
                    new Employee
                    {
                        FullName = "Karim Abdelaziz",
                        Email = "karim.abdelaziz@example.com",
                        PhoneNumber = "01000000005",
                        HireDate = new DateTime(2022, 11, 20),
                        Salary = 20000m,
                        DepartmentId = departmentsList.First(d => d.Name == "Information Technology").Id,
                        JobTitleId = jobTitlesList.First(j => j.Title == "IT Support Specialist").Id,
                        ProfileImagePath = "/uploads/employees/seed-5.jpg"
                    },
                };
                db.Employees.AddRange(employees);
                await db.SaveChangesAsync();
            }

            // Announcements ------------------------------------------------------
            if (!await db.Announcements.AnyAsync())
            {
                db.Announcements.AddRange(new[]
                {
                    new Announcement
                    {
                        Title = "Welcome to the team!",
                        Message = "We are excited to have you on board. Please complete your onboarding checklist on HR.",
                        PostedOn = DateTime.Now.AddDays(-1)
                    },
                    new Announcement
                    {
                        Title = "Salary Review Dates",
                        Message = "This year's salary review will be conducted during the first week of the new quarter.",
                        PostedOn = DateTime.Now.AddDays(-3)
                    },
                });
                await db.SaveChangesAsync();
            }
        }
    }
}
