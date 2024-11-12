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
            IEnumerable<Classes> classes = new List<Classes>()
            {
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
            };

            _context.Classes.AddRange(classes);
            _context.SaveChanges();
        }

        if (!_context.Courses.Any())
        {
            IEnumerable<Courses> courses = new List<Courses>()
            {
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
                    CourseCode = "AD100",
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
            };

            _context.Courses.AddRange(courses);
            _context.SaveChanges();
        }


        if (!_context.Rates.Any())
        {
            IEnumerable<Rates> rates = new List<Rates>()
            {
                new Rates
                {
                    Id = 1,
                    HourlyRate = 800,
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