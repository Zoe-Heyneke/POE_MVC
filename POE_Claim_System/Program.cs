using Microsoft.EntityFrameworkCore;
using POE_Claim_System.Models;  // Ensure this is the correct namespace for ClaimsContext
using POE_Claim_System.Services; // Ensure this is the correct namespace for ClaimService

var builder = WebApplication.CreateBuilder(args);

// Add services to the container (this replaces the old ConfigureServices method).


// Add MVC services (controllers with views)
builder.Services.AddControllersWithViews();

// Register your DbContext (ClaimsContext) with the connection string from appsettings.json
builder.Services.AddDbContext<ClaimsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register the ClaimService for Dependency Injection
builder.Services.AddScoped<ClaimService>();

//building app
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

app.UseAuthorization();     // Enable authorization middleware

// Set up the default controller route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();  // Run the application
