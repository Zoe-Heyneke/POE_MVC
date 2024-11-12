using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace POE_Claim_System.Models.Data
{
    public class ClaimStatusConfiguration : IEntityTypeConfiguration<ClaimStatus>
    {
        public void Configure(EntityTypeBuilder<ClaimStatus> builder)
        {
            builder.HasKey(b => b.Id);
            builder.HasData
            (
                new ClaimStatus
                {
                    Id = 1,
                    Status = "Approved"
                },
                  new ClaimStatus
                  {
                      Id = 2,
                      Status = "Rejected"
                  },
                    new ClaimStatus
                    {
                        Id = 3,
                        Status = "Pending"
                    }
                );

        }
    }
}
