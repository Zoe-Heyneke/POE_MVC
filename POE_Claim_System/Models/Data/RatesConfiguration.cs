using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace POE_Claim_System.Models.Data
{
    public class RatesConfiguration : IEntityTypeConfiguration<Rates>
    {
        public void Configure(EntityTypeBuilder<Rates> builder)
        {
            builder.HasKey(b => b.Id);
            builder.HasData
            (
                new Rates
                {
                    Id = 1,
                    HourlyRate = 800,
                }
                );
        }
    }
}
