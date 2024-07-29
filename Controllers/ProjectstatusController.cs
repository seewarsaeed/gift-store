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
    public class ProjectstatusController : Controller
    {
        private readonly ModelContext _context;

        public ProjectstatusController(ModelContext context)
        {
            _context = context;
        }

        // GET: Projectstatus
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Projectstatuses.Include(p => p.Present);
            return View(await modelContext.ToListAsync());
        }

        // GET: Projectstatus/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Projectstatuses == null)
            {
                return NotFound();
            }

            var projectstatus = await _context.Projectstatuses
                .Include(p => p.Present)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectstatus == null)
            {
                return NotFound();
            }

            return View(projectstatus);
        }

        // GET: Projectstatus/Create
        public IActionResult Create()
        {
            ViewData["Presentid"] = new SelectList(_context.Projectpresents, "Id", "Id");
            return View();
        }

        // POST: Projectstatus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Arrivedstatus,Paidstatus,Requeststatus,Presentid,Notifecationstatus")] Projectstatus projectstatus)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projectstatus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Presentid"] = new SelectList(_context.Projectpresents, "Id", "Id", projectstatus.Presentid);
            return View(projectstatus);
        }

        // GET: Projectstatus/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Projectstatuses == null)
            {
                return NotFound();
            }

            var projectstatus = await _context.Projectstatuses.FindAsync(id);
            if (projectstatus == null)
            {
                return NotFound();
            }
            ViewData["Presentid"] = new SelectList(_context.Projectpresents, "Id", "Id", projectstatus.Presentid);
            return View(projectstatus);
        }

        // POST: Projectstatus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Arrivedstatus,Paidstatus,Requeststatus,Presentid,Notifecationstatus")] Projectstatus projectstatus)
        {
            if (id != projectstatus.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectstatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectstatusExists(projectstatus.Id))
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
            ViewData["Presentid"] = new SelectList(_context.Projectpresents, "Id", "Id", projectstatus.Presentid);
            return View(projectstatus);
        }

        // GET: Projectstatus/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Projectstatuses == null)
            {
                return NotFound();
            }

            var projectstatus = await _context.Projectstatuses
                .Include(p => p.Present)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectstatus == null)
            {
                return NotFound();
            }

            return View(projectstatus);
        }

        // POST: Projectstatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Projectstatuses == null)
            {
                return Problem("Entity set 'ModelContext.Projectstatuses'  is null.");
            }
            var projectstatus = await _context.Projectstatuses.FindAsync(id);
            if (projectstatus != null)
            {
                _context.Projectstatuses.Remove(projectstatus);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectstatusExists(decimal id)
        {
          return (_context.Projectstatuses?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
