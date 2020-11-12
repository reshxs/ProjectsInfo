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

        public DbSet<Project> Projects { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
