using Microsoft.EntityFrameworkCore;
using ProjectsInfo.Models;

namespace ProjectsInfo.Data
{
    public class ProjectsInfoContext : DbContext
    {
        public ProjectsInfoContext(DbContextOptions<ProjectsInfoContext> options)
            : base (options)
        {
        }

        public DbSet<Project> Project { get; set; }
    }
}
