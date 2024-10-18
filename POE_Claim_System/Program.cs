using Microsoft.EntityFrameworkCore;
using POE_Claim_System.Models;
using POE_Claim_System.Models.Data;
using POE_Claim_System.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register ClaimsContext which includes Identity
/*
builder.Services.AddDbContext<ClaimsContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("YourConnectionString"),
        new MySqlServerVersion(new Version(8, 0, 27))
    ));
*/
// Register your ClaimService
builder.Services.AddDbContext<ClaimsContext>();
builder.Services.AddScoped<ClaimService>();
builder.Services.AddTransient<DataSeeder>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true; // Make the session cookie HTTP only
    options.Cookie.IsEssential = true; // Mark the session as essential
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Ensure authentication and authorization middleware are added
app.UseAuthentication(); // Uncomment if using authentication later
app.UseAuthorization();  // Ensure this is included

app.UseSession();
// Map controller routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    seeder.SeedData();
}
// Map Razor Pages if necessary
// app.MapRazorPages(); // Uncomment if using Razor Pages

app.Run();
