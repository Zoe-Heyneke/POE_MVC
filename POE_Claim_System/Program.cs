using Microsoft.EntityFrameworkCore;
using POE_Claim_System.Models;  // Make sure this namespace is correct for ClaimContext
using POE_Claim_System.Services; // Add this if ClaimService is in the Services namespace

var builder = WebApplication.CreateBuilder(args);

// Add ClaimContext with a connection string (ensure it's set in appsettings.json)
builder.Services.AddDbContext<ClaimsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("YourConnectionString")));

// Register the ClaimService for Dependency Injection
builder.Services.AddScoped<ClaimService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // HSTS for production scenarios
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Set up the default controller route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
