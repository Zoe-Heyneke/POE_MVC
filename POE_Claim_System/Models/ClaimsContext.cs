using Microsoft.EntityFrameworkCore;

namespace POE_Claim_System.Models
{
    public class ClaimsContext : DbContext
    {
        public DbSet<Claim> Claims { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Person> Persons { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<ClaimStatus> ClaimsStatus { get; set; }

        public DbSet<Document> Documents { get; set; }

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
            modelBuilder.Entity<Person>().HasKey(x => x.Id);

            modelBuilder.Entity<Claim>(entity =>
            {
                //claim has primary id
                //claim is one person with many claims
                entity.HasKey(x => x.Id);
                entity.HasOne(p => p.Person).WithMany(x => x.Claims);
            });
        }
    }
}
