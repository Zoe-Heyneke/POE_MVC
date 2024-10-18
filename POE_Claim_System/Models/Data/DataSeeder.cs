namespace POE_Claim_System.Models.Data;

public class DataSeeder
{
    private readonly ClaimsContext _context;
    public DataSeeder(ClaimsContext context)
    {
        _context = context;
    }



    public void SeedData()
    {
        if (!_context.Classes.Any())
        {
            IEnumerable<Class> classes = new List<Class>()
            {
                new Class
                {
                    Id = 1,
                    ClassName = "Group 1"
                },
                new Class
                {
                    Id = 2,
                    ClassName = "Group 2"
                },
                new Class
                {
                    Id = 3,
                    ClassName = "Group 3"
                }
            };

            _context.Classes.AddRange(classes);
            _context.SaveChanges();
        }

        if (!_context.Courses.Any())
        {
            IEnumerable<Course> courses = new List<Course>()
            {
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
                    CourseCode = "AD100",
                    Name = "Advanced Databases",
                    Timestamp = DateTime.UtcNow,
                }

            };

            _context.Courses.AddRange(courses);
            _context.SaveChanges();
        }


        if (!_context.Rates.Any())
        {
            IEnumerable<Rate> rates = new List<Rate>()
            {
                new Rate
                {
                    Id = 1,
                    HourlyRate = 1000,
                }

            };

            _context.Rates.AddRange(rates);
            _context.SaveChanges();
        }

        if (!_context.ClaimStatuses.Any())
        {
            IEnumerable<ClaimStatus> claimStatus = new List<ClaimStatus>()
            {
                new ClaimStatus
                {
                    Id = 1,
                    Status = "Pending"
                },
                new ClaimStatus
                {
                    Id = 2,
                    Status = "Approved"
                },
                new ClaimStatus
                {
                    Id = 3,
                    Status = "Rejected"
                }

            };

            _context.ClaimStatuses.AddRange(claimStatus);
            _context.SaveChanges();
        }
    }


}