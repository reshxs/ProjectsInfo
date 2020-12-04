using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectsInfo.Data;
using ProjectsInfo.Models;

namespace ProjectsInfo.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ProjectsInfoContext _context;

        public ProjectController(ProjectsInfoContext context)
        {
            _context = context;
        }

        // GET: Project
        public async Task<IActionResult> Index()
        {
            return View(await _context.Projects.ToListAsync());
        }

        // GET: Project/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .FirstOrDefaultAsync(m => m.ID == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Project/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Project/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("ID,Title,StartDate,EndDate,ExpectedHours,DevelopmentHourPrice,TestingHours,TestingHourPrice")]
            Project project)
        {
            if (ModelState.IsValid)
            {
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(project);
        }

        // GET: Project/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Project/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("ID,Title,StartDate,EndDate,ExpectedHours,DevelopmentHourPrice,TestingHours,TestingHourPrice")]
            Project project)
        {
            if (id != project.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.ID))
                    {
                        return NotFound();
                    }
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(project);
        }

        //GET: Project/EditDevelopers/5
        public async Task<IActionResult> EditDevelopers(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.DeveloperAssignments)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (project == null)
            {
                return NotFound();
            }

            PopulateAssignedDeveloperData(project);
            return View(project);
        }
        
        private void PopulateAssignedDeveloperData(Project project)
        {
            var allDevelopers = _context.Developers;
            var projectDevelopers = new HashSet<int>(
                project.DeveloperAssignments.Select(d => d.DeveloperID));
            var viewModel = new List<AssignedDeveloperData>();
            
            foreach (var developer in allDevelopers)
            {
                viewModel.Add(new AssignedDeveloperData()
                {
                    DeveloperID = developer.ID,
                    Name = developer.Name,
                    Assigned = projectDevelopers.Contains(developer.ID)
                });
            }

            ViewData["Developers"] = viewModel;
        }

        // POST: Project/EditDevelopers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDevelopers(int? id, string[] selectedDevelopers)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectToUpdate = await _context.Projects
                .Include(p => p.DeveloperAssignments)
                .ThenInclude(p => p.Developer)
                .FirstOrDefaultAsync(m => m.ID == id);
            
            if (await TryUpdateModelAsync(
                projectToUpdate,
                "",
                p => p.Title, p => p.StartDate, p => p.EndDate, p => p.ExpectedHours, p => p.DevelopmentHourPrice,
                p => p.TestingHours, p => p.TestingHourPrice
            ))
            {
                UpdateProjectDevelopers(selectedDevelopers, projectToUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                                                 "Try again, and if the problem persists, " +
                                                 "see your system administrator.");
                }   
            }
            UpdateProjectDevelopers(selectedDevelopers, projectToUpdate);
            PopulateAssignedDeveloperData(projectToUpdate);
            return RedirectToAction(nameof(EditDevelopers));
        }

        private void UpdateProjectDevelopers(string[] selectedDevelopers, Project projectToUpdate)
        {
            if (selectedDevelopers == null)
            {
                projectToUpdate.DeveloperAssignments = new List<DeveloperAssignment>();
                return;
            }
            
            var selectedDevelopersHs = new HashSet<string>(selectedDevelopers);
            var projectDevelopers = new HashSet<int>(
                projectToUpdate.DeveloperAssignments.Select(d => d.Developer.ID));
            foreach (var developer in _context.Developers)
            {
                if (selectedDevelopersHs.Contains(developer.ID.ToString()))
                {
                    if (!projectDevelopers.Contains(developer.ID))
                    {
                        var newDeveloper = new DeveloperAssignment()
                        {
                            ProjectID = projectToUpdate.ID,
                            DeveloperID = developer.ID,
                            Project = projectToUpdate,
                            Developer = developer,
                        };
                        projectToUpdate.DeveloperAssignments.Add(newDeveloper);
                        UpdateMonthsForDeveloper(newDeveloper);
                    }
                }
                else
                {
                    if (projectDevelopers.Contains(developer.ID))
                    {
                        var developerToRemove = projectToUpdate.DeveloperAssignments.FirstOrDefault(
                            d => d.DeveloperID == developer.ID);
                        _context.Remove(developerToRemove!);
                    }
                }
            }
        }

        private void UpdateMonthsForDeveloper(DeveloperAssignment developerAssignment)
        {
            var startDate = developerAssignment.Project.StartDate;
            var startMonth = startDate.Month;
            var startYear = startDate.Year;
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            developerAssignment.Months ??= new List<Month>();
            var developerMonths = developerAssignment.Months
                .Select(month => month.Date)
                .ToHashSet();
            
            //TODO: Добавляется на один месяц меньше, чем надо!
            while (startMonth != currentMonth || startYear != currentYear)
            {
                var newDate = DateTime.Parse($"{startYear}-{startMonth}-1");
                if (!developerMonths.Contains(newDate))
                {
                    var newMonth = new Month()
                    {
                        Date = newDate,
                        Hours = 0,
                        DeveloperAssignmentID = developerAssignment.ID,
                        DeveloperAssignment = developerAssignment
                    };
                    developerAssignment.Months.Add(newMonth);
                }

                startMonth++;
                if (startMonth > 12)
                {
                    startYear++;
                    startMonth = 1;
                }
            }
        }

        // GET: Project/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .FirstOrDefaultAsync(m => m.ID == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.ID == id);
        }
    }
}