using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GiftStore.Models;

namespace GiftStore.Controllers
{
    public class ProjectfootersController : Controller
    {
        private readonly ModelContext _context;

        public ProjectfootersController(ModelContext context)
        {
            _context = context;
        }

        // GET: Projectfooters
        public async Task<IActionResult> Index()
        {
              return _context.Projectfooters != null ? 
                          View(await _context.Projectfooters.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Projectfooters'  is null.");
        }

        // GET: Projectfooters/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Projectfooters == null)
            {
                return NotFound();
            }

            var projectfooter = await _context.Projectfooters
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectfooter == null)
            {
                return NotFound();
            }

            return View(projectfooter);
        }

        // GET: Projectfooters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Projectfooters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Copyright")] Projectfooter projectfooter)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projectfooter);
                await _context.SaveChangesAsync();
                return RedirectToAction("ManageWebsitePages", "Admin");
            }
            return View(projectfooter);
        }

        // GET: Projectfooters/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Projectfooters == null)
            {
                return NotFound();
            }

            var projectfooter = await _context.Projectfooters.FindAsync(id);
            if (projectfooter == null)
            {
                return NotFound();
            }
            return View(projectfooter);
        }

        // POST: Projectfooters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Copyright")] Projectfooter projectfooter)
        {
            if (id != projectfooter.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectfooter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectfooterExists(projectfooter.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("ManageWebsitePages", "Admin");
            }
            return View(projectfooter);
        }

        // GET: Projectfooters/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Projectfooters == null)
            {
                return NotFound();
            }

            var projectfooter = await _context.Projectfooters
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectfooter == null)
            {
                return NotFound();
            }

            return View(projectfooter);
        }

        // POST: Projectfooters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Projectfooters == null)
            {
                return Problem("Entity set 'ModelContext.Projectfooters'  is null.");
            }
            var projectfooter = await _context.Projectfooters.FindAsync(id);
            if (projectfooter != null)
            {
                _context.Projectfooters.Remove(projectfooter);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("ManageWebsitePages", "Admin");
        }

        private bool ProjectfooterExists(decimal id)
        {
          return (_context.Projectfooters?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
