using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace POE_Claim_System.Models.Data
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(b => b.Id);
            builder.HasData
            (
                new Course
                {
                    Id = 1,
                    CourseCode = "PROG6212",
                    Name = "Programming 2B",
                    Timestamp = DateTime.UtcNow,
                },
                  new Course
                  {
                      Id = 2,
                      CourseCode = "AD7311",
                      Name = "Advanced Databases",
                      Timestamp = DateTime.UtcNow,
                  },
                  new Course
                  {
                      Id = 1,
                      CourseCode = "WEDE6021",
                      Name = "Web Development (Intermediate)",
                      Timestamp = DateTime.UtcNow,
                  },
                  new Course
                  {
                      Id = 1,
                      CourseCode = "SOEN6222",
                      Name = "Software Engineering",
                      Timestamp = DateTime.UtcNow,
                  }
                );

        }
    }
}
