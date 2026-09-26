using ITI_GRADUATION.Data;
using ITI_GRADUATION.Models;
using ITI_GRADUATION.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); // Enables Razor Pages (e.g. the Announcements feature) alongside MVC controllers/views.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, SmtpEmailService>();

var app = builder.Build();

// Make sure the folder used to store uploaded employee profile images exists.
var uploadsPath = Path.Combine(app.Environment.WebRootPath, "uploads", "employees");
Directory.CreateDirectory(uploadsPath);

// Apply any pending EF migrations, then seed starter data (departments, job
// titles, employees with profile images, announcements) if the DB is empty.
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
    await DbSeeder.SeedAsync(dbContext);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

// UseStaticFiles is required in addition to MapStaticAssets so that files
// uploaded at runtime (e.g. employee profile images saved under wwwroot/uploads)
// are served correctly - MapStaticAssets only covers build-time static assets.
app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
    .WithStaticAssets();

app.Run();
