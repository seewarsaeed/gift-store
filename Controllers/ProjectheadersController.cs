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
    public class ProjectheadersController : Controller
    {
        private readonly ModelContext _context;

        public ProjectheadersController(ModelContext context)
        {
            _context = context;
        }

        // GET: Projectheaders
        public async Task<IActionResult> Index()
        {
              return _context.Projectheaders != null ? 
                          View(await _context.Projectheaders.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Projectheaders'  is null.");
        }

        // GET: Projectheaders/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Projectheaders == null)
            {
                return NotFound();
            }

            var projectheader = await _context.Projectheaders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectheader == null)
            {
                return NotFound();
            }

            return View(projectheader);
        }

        // GET: Projectheaders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Projectheaders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Projectheader projectheader)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projectheader);
                await _context.SaveChangesAsync();
                return RedirectToAction("ManageWebsitePages", "Admin");
            }
            return View(projectheader);
        }

        // GET: Projectheaders/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Projectheaders == null)
            {
                return NotFound();
            }

            var projectheader = await _context.Projectheaders.FindAsync(id);
            if (projectheader == null)
            {
                return NotFound();
            }
            return View(projectheader);
        }

        // POST: Projectheaders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Name")] Projectheader projectheader)
        {
            if (id != projectheader.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    _context.Update(projectheader);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectheaderExists(projectheader.Id))
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
            return View(projectheader);
        }

        // GET: Projectheaders/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Projectheaders == null)
            {
                return NotFound();
            }

            var projectheader = await _context.Projectheaders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectheader == null)
            {
                return NotFound();
            }

            return View(projectheader);
        }

        // POST: Projectheaders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Projectheaders == null)
            {
                return Problem("Entity set 'ModelContext.Projectheaders'  is null.");
            }
            var projectheader = await _context.Projectheaders.FindAsync(id);
            if (projectheader != null)
            {
                _context.Projectheaders.Remove(projectheader);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("ManageWebsitePages", "Admin");
        }

        private bool ProjectheaderExists(decimal id)
        {
          return (_context.Projectheaders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
