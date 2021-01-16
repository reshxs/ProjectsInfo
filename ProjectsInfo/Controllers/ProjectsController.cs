using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectsInfo.Data;
using ProjectsInfo.Models;
using ProjectsInfo.Models.Accounts;

namespace ProjectsInfo.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly ProjectsInfoContext _context;

        public ProjectsController(ProjectsInfoContext context)
        {
            _context = context;
        }

        // GET: Project
        public async Task<IActionResult> Index()
        {
            var projects = await _context.Projects
                .Include(p => p.Manager)
                .Include(p => p.DeveloperAssignments)
                .ThenInclude(d => d.Months)
                .Include(p => p.DeveloperAssignments)
                .ThenInclude(p => p.Developer)
                .ToListAsync();
            return View(projects);
        }

        // GET: Project/Details/5
        [Authorize(Roles=UserRoles.Manager)]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.DeveloperAssignments)
                .ThenInclude(d => d.Months)
                .Include(p => p.DeveloperAssignments)
                .ThenInclude(p => p.Developer)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Project/Create
        [Authorize(Roles=UserRoles.Manager)]
        public IActionResult Create()
        {
            PopulateManagersDropDownList();
            return View();
        }

        // POST: Project/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles=UserRoles.Manager)]
        public async Task<IActionResult> Create(
            [Bind("ID,Title,StartDate,EndDate,ExpectedHours,DevelopmentHourPrice,TestingHours,TestingHourPrice, ManagerID")]
            Project project)
        {
            if (ModelState.IsValid)
            {
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateManagersDropDownList(project.ManagerID);
            return View(project);
        }

        // GET: Project/Edit/5
        [Authorize(Roles=UserRoles.Manager)]
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
            
            PopulateManagersDropDownList(project.ManagerID);
            return View(project);
        }

        // POST: Project/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles=UserRoles.Manager)]
        public async Task<IActionResult> Edit(int id,
            [Bind("ID,Title,StartDate,EndDate,ExpectedHours,DevelopmentHourPrice,TestingHours,TestingHourPrice, ManagerID")]
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

            PopulateManagersDropDownList(project.ManagerID);
            return View(project);
        }

        private void PopulateManagersDropDownList(object selectedManagers = null)
        {
            var managersQuery = 
                from m in _context.Managers
                orderby m.Name
                select m;

            ViewBag.ManagerID = new SelectList(
                managersQuery.AsNoTracking(), 
                "ID", 
                "Name", 
                selectedManagers);
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
                    .ThenInclude(d => d.Months)
                .Include(p => p.DeveloperAssignments)
                    .ThenInclude(d => d.Developer)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (project == null)
            {
                return NotFound();
            }
            
            return View(project);
        }

        // GET: Project/Delete/5
        [Authorize(Roles=UserRoles.Manager)]
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
        [Authorize(Roles=UserRoles.Manager)]
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