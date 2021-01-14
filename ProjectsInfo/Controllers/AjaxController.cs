using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectsInfo.Data;
using ProjectsInfo.Models;
using ProjectsInfo.Models.Api;

namespace ProjectsInfo.Controllers
{
    public class AjaxController: Controller
    {
        private readonly ProjectsInfoContext _context;

        public AjaxController(ProjectsInfoContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public ActionResult<IEnumerable<AssignedDeveloperData>> GetNotAssignedDevelopers(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var project = _context.Projects
                .Include(p => p.DeveloperAssignments)
                .FirstOrDefault(p => p.ID == id);

            if (project == null)
            {
                return NotFound();
            }

            if (project.DeveloperAssignments == null)
            {
                project.DeveloperAssignments = new List<DeveloperAssignment>();
            }
            
            var allDevelopers = _context.Developers;
            var projectDevelopers = new HashSet<int>(
                project.DeveloperAssignments.Select(d => d.DeveloperID));
            return allDevelopers
                .Select(developer => new AssignedDeveloperData
                {
                    DeveloperID = developer.ID, 
                    Name = developer.Name, 
                    Assigned = projectDevelopers.Contains(developer.ID)
                })
                .Where(d => !d.Assigned)
                .ToList();
        }

        [HttpPost]
        public async Task<IActionResult> AddDeveloper(int? id, AddedDeveloper addedDeveloper)
        {
            if (id == null || id != addedDeveloper.ProjectId)
                return NotFound();

            var project = await _context.Projects
                .Include(p => p.DeveloperAssignments)
                .ThenInclude(d => d.Developer)
                .FirstOrDefaultAsync(p => p.ID == id);

            if (project == null)
                return NotFound();

            var developer = await _context.Developers.FindAsync(addedDeveloper.DeveloperId);

            if (developer == null)
                return NotFound();
            
            project.DeveloperAssignments ??= new List<DeveloperAssignment>();

            if (!project.DeveloperAssignments
                .Select(d => d.DeveloperID)
                .Contains(addedDeveloper.DeveloperId))
            {
                project.DeveloperAssignments.Add(new DeveloperAssignment
                {
                    DeveloperID = addedDeveloper.DeveloperId,
                    ProjectID = addedDeveloper.ProjectId,
                    Project = project,
                    Developer = developer
                });
                await _context.SaveChangesAsync();
                return Ok();
            }

            return Conflict();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveDeveloper(int? id, AddedDeveloper addedDeveloper)
        {
            if (id == null || addedDeveloper.ProjectId != id)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.DeveloperAssignments)
                .FirstOrDefaultAsync(p => p.ID == id);

            project.DeveloperAssignments ??= new List<DeveloperAssignment>();

            if (project.DeveloperAssignments
                .Select(d => d.DeveloperID)
                .Contains(addedDeveloper.DeveloperId))
            {
                project.DeveloperAssignments.Remove(
                    project.DeveloperAssignments
                        .First(d => d.DeveloperID == addedDeveloper.DeveloperId));
                await _context.SaveChangesAsync();
                return Ok();
            }

            return NotFound();
        }
    }
}