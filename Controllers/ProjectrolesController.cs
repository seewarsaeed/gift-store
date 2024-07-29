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
    public class ProjectrolesController : Controller
    {
        private readonly ModelContext _context;

        public ProjectrolesController(ModelContext context)
        {
            _context = context;
        }

        // GET: Projectroles
        public async Task<IActionResult> Index()
        {
              return _context.Projectroles != null ? 
                          View(await _context.Projectroles.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Projectroles'  is null.");
        }

        // GET: Projectroles/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Projectroles == null)
            {
                return NotFound();
            }

            var projectrole = await _context.Projectroles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectrole == null)
            {
                return NotFound();
            }

            return View(projectrole);
        }

        // GET: Projectroles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Projectroles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Projectrole projectrole)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projectrole);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(projectrole);
        }

        // GET: Projectroles/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Projectroles == null)
            {
                return NotFound();
            }

            var projectrole = await _context.Projectroles.FindAsync(id);
            if (projectrole == null)
            {
                return NotFound();
            }
            return View(projectrole);
        }

        // POST: Projectroles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Name")] Projectrole projectrole)
        {
            if (id != projectrole.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectrole);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectroleExists(projectrole.Id))
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
            return View(projectrole);
        }

        // GET: Projectroles/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Projectroles == null)
            {
                return NotFound();
            }

            var projectrole = await _context.Projectroles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectrole == null)
            {
                return NotFound();
            }

            return View(projectrole);
        }

        // POST: Projectroles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Projectroles == null)
            {
                return Problem("Entity set 'ModelContext.Projectroles'  is null.");
            }
            var projectrole = await _context.Projectroles.FindAsync(id);
            if (projectrole != null)
            {
                _context.Projectroles.Remove(projectrole);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectroleExists(decimal id)
        {
          return (_context.Projectroles?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
