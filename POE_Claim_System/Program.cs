using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using POE_Claim_System.Models;
using POE_Claim_System.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register ClaimsContext which includes Identity
builder.Services.AddDbContext<ClaimsContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("YourConnectionString"),
        new MySqlServerVersion(new Version(8, 0, 27))
    ));

// Register Identity services using the same ClaimsContext
builder.Services.AddDefaultIdentity<POE_Claim_SystemUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ClaimsContext>();

// Register your ClaimService
builder.Services.AddScoped<ClaimService>();

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
app.UseAuthentication();
app.UseAuthorization();

// Map controller routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Additional routes for account actions if necessary
app.MapRazorPages(); // This is required if you're using Razor Pages for Identity

app.Run();
