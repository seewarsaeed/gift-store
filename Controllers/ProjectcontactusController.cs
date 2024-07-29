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
    public class ProjectcontactusController : Controller
    {
        private readonly ModelContext _context;

        public ProjectcontactusController(ModelContext context)
        {
            _context = context;
        }

        // GET: Projectcontactus
        public async Task<IActionResult> Index()
        {
              return _context.Projectcontactus != null ? 
                          View(await _context.Projectcontactus.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Projectcontactus'  is null.");
        }

        // GET: Projectcontactus/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Projectcontactus == null)
            {
                return NotFound();
            }

            var projectcontactu = await _context.Projectcontactus
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectcontactu == null)
            {
                return NotFound();
            }

            return View(projectcontactu);
        }

        // GET: Projectcontactus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Projectcontactus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,Pnumber,Address")] Projectcontactu projectcontactu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projectcontactu);
                await _context.SaveChangesAsync();
                return RedirectToAction("ManageWebsitePages", "Admin");
            }
            return View(projectcontactu);
        }

        // GET: Projectcontactus/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Projectcontactus == null)
            {
                return NotFound();
            }

            var projectcontactu = await _context.Projectcontactus.FindAsync(id);
            if (projectcontactu == null)
            {
                return NotFound();
            }
            return View(projectcontactu);
        }

        // POST: Projectcontactus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Email,Pnumber,Address")] Projectcontactu projectcontactu)
        {
            if (id != projectcontactu.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectcontactu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectcontactuExists(projectcontactu.Id))
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
            return View(projectcontactu);
        }

        // GET: Projectcontactus/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Projectcontactus == null)
            {
                return NotFound();
            }

            var projectcontactu = await _context.Projectcontactus
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectcontactu == null)
            {
                return NotFound();
            }

            return View(projectcontactu);
        }

        // POST: Projectcontactus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Projectcontactus == null)
            {
                return Problem("Entity set 'ModelContext.Projectcontactus'  is null.");
            }
            var projectcontactu = await _context.Projectcontactus.FindAsync(id);
            if (projectcontactu != null)
            {
                _context.Projectcontactus.Remove(projectcontactu);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("ManageWebsitePages", "Admin");
        }

        private bool ProjectcontactuExists(decimal id)
        {
          return (_context.Projectcontactus?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
