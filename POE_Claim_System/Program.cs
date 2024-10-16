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

app.UseAuthentication();  // Add authentication middleware
app.UseAuthorization();   // Add authorization middleware

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
