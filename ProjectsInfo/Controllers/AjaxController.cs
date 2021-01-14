using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectsInfo.Data;
using ProjectsInfo.Models;

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
            var project = _context.Projects.Find(id);

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
    }
}