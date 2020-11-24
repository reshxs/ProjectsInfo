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
                .HasMany(project => project.Developers)
                .WithOne(projectAssigment => projectAssigment.Project)
                .IsRequired();

            //One-to-Many (developer <- projectAssigment)
            modelBuilder.Entity<Developer>()
                .HasMany(developer => developer.Projects)
                .WithOne(projectAssigment => projectAssigment.Developer)
                .IsRequired();
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Developer> Developers { get; set; }
        public DbSet<ProjectAssigment> ProjectAssigments { get; set; }
    }
}
