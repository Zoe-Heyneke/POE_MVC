using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace POE_Claim_System.Models.Data
{
    public class RateConfiguration : IEntityTypeConfiguration<Rate>
    {
        public void Configure(EntityTypeBuilder<Rate> builder)
        {
            builder.HasKey(b => b.Id);
            builder.HasData
            (
                new Rate
                {
                    Id = 1,
                    HourlyRate = 1000,
                }
                );

        }
    }
}
