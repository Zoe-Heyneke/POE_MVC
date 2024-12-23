﻿using Microsoft.EntityFrameworkCore;
using POE_Claim_System.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using POE_Claim_System.Models.Data;
using Microsoft.Extensions.Options;
using System.Configuration;

namespace POE_Claim_System.Models
{
    public class ClaimsContext : DbContext
    {
        public ClaimsContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Claim> Claims { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Person> Persons { get; set; }

        public DbSet<Courses> Courses { get; set; }

        public DbSet<Classes> Classes { get; set; }

        public DbSet<ClaimStatus> ClaimStatuses { get; set; }

        public DbSet<Document> Documents { get; set; }

        public DbSet<Rates> Rates { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseInMemoryDatabase(databaseName: "Claims");
            // connect to sqlite database
            optionsBuilder.UseSqlite("Data Source=Claims.db");
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

            modelBuilder.ApplyConfiguration(new RatesConfiguration());
            modelBuilder.ApplyConfiguration(new ClaimStatusConfiguration());
            modelBuilder.ApplyConfiguration(new ClassesConfiguration());
            modelBuilder.ApplyConfiguration(new CoursesConfiguration());
            modelBuilder.Entity<Role>().HasKey(r => r.Id);
        }


    }
}
