using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace POE_Claim_System.Models.Data
{
    public class ClassesConfiguration : IEntityTypeConfiguration<Classes>
    {
        public void Configure(EntityTypeBuilder<Classes> builder)
        {
            builder.HasKey(b => b.Id);
            builder.HasData
            (
                new Classes
                {
                    Id = 1,
                    ClassName = "Group 1"
                },
                  new Classes
                  {
                      Id = 2,
                      ClassName = "Group 2"
                  },
                  new Classes
                  {
                      Id = 3,
                      ClassName = "Group 3"
                  }
                );

        }
    }
}
