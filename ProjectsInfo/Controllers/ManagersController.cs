using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectsInfo.Data;
using ProjectsInfo.Models.Employes;

namespace ProjectsInfo.Controllers
{
    public class ManagersController : Controller
    {
        private readonly ProjectsInfoContext _context;

        public ManagersController(ProjectsInfoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var managers = await _context.Managers.ToListAsync();
            return View(managers);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Name,Salary")] Manager manager)
        {
            if (ModelState.IsValid)
            {
                _context.Add(manager);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(manager);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var manager = await _context.Managers.FirstOrDefaultAsync(m => m.ID == id);
            if (manager == null)
            {
                return NotFound();
            }
            
            return View(manager);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Salary")] Manager manager)
        {
            if (id != manager.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(manager);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ManagerExists(manager.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            
            return View(manager);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manager = await _context.Managers.FirstOrDefaultAsync(m => m.ID == id);

            if (manager == null)
            {
                return NotFound();
            }

            return View(manager);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var manager = await _context.Managers.FindAsync(id);
            _context.Managers.Remove(manager);
            await _context.SaveChangesAsync();
            return Redirect(nameof(Index));
        }

        private async Task<bool> ManagerExists(int id)
        {
            return await _context.Managers.AnyAsync(m => m.ID == id);
        }
    }
}