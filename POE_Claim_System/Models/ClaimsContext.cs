using Microsoft.EntityFrameworkCore;
using POE_Claim_System.Models;

namespace POE_Claim_System.Models
{
    public class ClaimsContext : DbContext
    {
        public DbSet<Claim> Claims { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Person> Persons { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<ClaimStatus> ClaimStatuses { get; set; }

        public DbSet<Document> Documents { get; set; }

        public DbSet<Rate> Rates { get; set; }

        //when configuring
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //connection string
            optionsBuilder.UseMySQL("server=localhost;database=Claims;user=root;password=password");
        }


        //model create model in code and put to db
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //specify relationships
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Person>().HasKey(p => p.Id);

            modelBuilder.Entity<Claim>(entity =>
            {
                //claim has primary id
                //claim is one person with many claims
                entity.HasKey(c => c.Id);
                entity.HasOne(c => c.Person).WithMany(p => p.Claims).HasForeignKey(c => c.PersonId);

                entity.HasOne(c => c.Course).WithMay().HasForeignKey(c => c.CourseId);
            });

            modelBuilder.Entity<Course>().HasKey(c => c.Id);
            modelBuilder.Entity<Role>().HasKey(r => r.Id);
        }


    }
}
