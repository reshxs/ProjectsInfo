using System;
using ProjectsInfo.Models;
using Npgsql.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ProjectsInfo.Data
{
    public class ProjectsInfoContext : DbContext
    {
        public ProjectsInfoContext(DbContextOptions<ProjectsInfoContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
        public DbSet<User> Users { get; set; }
        public DbSet<Developer> Developers { get; set; }
        public DbSet<DeveloperAssignment> DeveloperAssignments { get; set; }
        public DbSet<Month> Months { get; set; }
    }
}
