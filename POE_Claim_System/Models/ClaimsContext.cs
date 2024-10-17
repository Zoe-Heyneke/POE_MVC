using Microsoft.EntityFrameworkCore;
using POE_Claim_System.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace POE_Claim_System.Models
{
    public class ClaimsContext : DbContext
    {
        //constructor for ClaimsCOntext to accept Db
        public ClaimsContext(DbContextOptions<ClaimsContext> options)
            : base(options)
        {
        }
        public DbSet<Claim> Claims { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Person> Persons { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<ClaimStatus> ClaimStatuses { get; set; }

        public DbSet<Document> Documents { get; set; }

        public DbSet<Rate> Rates { get; set; }

        public DbSet<User> Users { get; set; }

        //when configuring
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //connection string
         //   optionsBuilder.UseMySQL("server=localhost;database=Claims_DB;user=root;password=");
         optionsBuilder.UseInMemoryDatabase(databaseName: "Claims");
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

                entity.HasOne(c => c.Course).WithMany().HasForeignKey(c => c.CourseId);
            });

            modelBuilder.Entity<Course>().HasKey(c => c.Id);
            modelBuilder.Entity<Role>().HasKey(r => r.Id);
        }


    }
}
