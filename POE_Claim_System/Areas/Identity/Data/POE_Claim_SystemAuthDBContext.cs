using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using POE_Claim_System.Areas.Identity.Data;

namespace POE_Claim_System.Data;

public class POE_Claim_SystemAuthDBContext : IdentityDbContext<POE_Claim_SystemUser>
{
    public POE_Claim_SystemAuthDBContext(DbContextOptions<POE_Claim_SystemAuthDBContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
