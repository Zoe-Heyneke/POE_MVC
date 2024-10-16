using Microsoft.EntityFrameworkCore;
using POE_Claim_System.Models;  // Ensure this is the correct namespace for ClaimsContext and POE_Claim_SystemUser
using POE_Claim_System.Services; // Ensure this is the correct namespace for ClaimService
using Microsoft.AspNetCore.Identity;
using POE_Claim_System.Data; // Ensure this is the correct namespace for POE_Claim_SystemAuthDBContext

var builder = WebApplication.CreateBuilder(args);

// Add services to the container (this replaces the old ConfigureServices method).
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ClaimsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("YourConnectionString")));

builder.Services.AddDefaultIdentity<POE_Claim_SystemUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<POE_Claim_SystemAuthDBContext>();

// Register your DbContext (ClaimsContext) with the connection string from appsettings.json
builder.Services.AddDbContext<ClaimsContext>(options =>
{
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 27)) // Use the version of your MySQL server
    );
});

// Register the ClaimService for Dependency Injection
builder.Services.AddScoped<ClaimService>();

// Building the app
var app = builder.Build();

// Configure the HTTP request pipeline (this replaces the old Configure method).
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();  // HSTS for production environments
}

app.UseHttpsRedirection();  // Enforce HTTPS redirection
app.UseStaticFiles();       // Serve static files from wwwroot

app.UseRouting();           // Enable routing for incoming requests

app.UseAuthentication();    // Enable authentication middleware (Identity)
app.UseAuthorization();     // Enable authorization middleware

// Set up the default controller route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();  // Run the application
