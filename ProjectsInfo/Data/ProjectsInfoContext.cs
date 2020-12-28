using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ProjectsInfo.Models;
using Npgsql.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectsInfo.Models.Accounts;

namespace ProjectsInfo.Data
{
    public class ProjectsInfoContext : IdentityDbContext<ApplicationUser>
    {
        public ProjectsInfoContext(DbContextOptions<ProjectsInfoContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //One-to-Many (project <- projectAssigment)
            modelBuilder.Entity<Project>()
                .HasMany(project => project.DeveloperAssignments)
                .WithOne(projectAssigment => projectAssigment.Project)
                .IsRequired();

            //One-to-Many (developer <- projectAssigment)
            modelBuilder.Entity<Developer>()
                .HasMany(developer => developer.DeveloperAssignments)
                .WithOne(projectAssigment => projectAssigment.Developer)
                .IsRequired();
            
            //Primary key for DeveloperAssignment
            modelBuilder.Entity<DeveloperAssignment>()
                .HasKey(d => new {d.ProjectID, d.DeveloperID});

            //One-to-Many (projectAssigment <- month)
            modelBuilder.Entity<DeveloperAssignment>()
                .HasMany(projectAssigment => projectAssigment.Months)
                .WithOne(month => month.DeveloperAssignment)
                .IsRequired();
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Developer> Developers { get; set; }
        public DbSet<DeveloperAssignment> DeveloperAssignments { get; set; }
        public DbSet<Month> Months { get; set; }
        public DbSet<Manager> Managers { get; set; }
    }
}
