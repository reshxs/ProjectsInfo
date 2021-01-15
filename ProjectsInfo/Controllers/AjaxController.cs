using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
                    Developer = developer,
                    Months = new List<Month>()
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

        [HttpPost]
        public async Task<IActionResult> UpdateMonth(int? id, UpdateMonthModel month)
        {
            var monthToUpdate = await _context.Months.FirstOrDefaultAsync(m => m.ID == id);

            if (monthToUpdate == null || month.Id != id)
            {
                return NotFound();
            }
            
            monthToUpdate.Hours = month.Hours;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<UpdateMonthModel>> AddMonth(int? id, AddMonthModel month)
        {
            var project = await _context.Projects
                .Include(p => p.DeveloperAssignments)
                .ThenInclude(d => d.Months)
                .FirstOrDefaultAsync(p => p.ID == id);

            if (project == null || month.ProjectId != id)
            {
                return NotFound();
            }

            var developerAssignment =
                project.DeveloperAssignments.FirstOrDefault(d => d.DeveloperID == month.DeveloperId);

            if (developerAssignment == null)
            {
                return NotFound();
            }

            var splitDate = month.Date.Split("/");
            var monthNumber = splitDate[0];
            var year = splitDate[1];
            var monthToAdd = new Month()
            {
                Hours = month.Hours,
                Date = DateTime.Parse($"{year}-{monthNumber}-{1}"),
                DeveloperAssignment = developerAssignment,
                DeveloperAssignmentID = developerAssignment.ID
            };
            
            developerAssignment.Months.Add(monthToAdd);
            await _context.SaveChangesAsync();
            return new UpdateMonthModel
            {
                Hours = monthToAdd.Hours,
                Id = monthToAdd.ID
            };
        }
    }
}