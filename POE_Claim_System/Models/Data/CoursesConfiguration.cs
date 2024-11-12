using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace POE_Claim_System.Models.Data
{
    public class CoursesConfiguration : IEntityTypeConfiguration<Courses>
    {
        public void Configure(EntityTypeBuilder<Courses> builder)
        {
            builder.HasKey(b => b.Id);
            builder.HasData
            (
                new Courses
                {
                    Id = 1,
                    CourseCode = "PROG6212",
                    Name = "Programming 2B",
                    Timestamp = DateTime.UtcNow,
                },
                  new Courses
                  {
                      Id = 2,
                      CourseCode = "AD7311",
                      Name = "Advanced Databases",
                      Timestamp = DateTime.UtcNow,
                  },
                  new Courses
                  {
                      Id = 3,
                      CourseCode = "WEDE6021",
                      Name = "Web Development (Intermediate)",
                      Timestamp = DateTime.UtcNow,
                  },
                  new Courses
                  {
                      Id = 4,
                      CourseCode = "SOEN6222",
                      Name = "Software Engineering",
                      Timestamp = DateTime.UtcNow,
                  }
                );

        }
    }
}
