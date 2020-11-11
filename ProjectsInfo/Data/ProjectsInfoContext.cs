using System;
using ProjectsInfo.Models;
using Npgsql.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ProjectsInfo.Data
{
    public class ProjectsInfoContext : DbContext
    {
        public ProjectsInfoContext()
        {
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ProjectTask> Tasks { get; set; }

        //TODO change DB options
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5432;Database=usersdb;Username=postgres;Password=password");
    }
}
