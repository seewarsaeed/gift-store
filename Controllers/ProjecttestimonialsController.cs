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
    public class ProjecttestimonialsController : Controller
    {
        private readonly ModelContext _context;

        public ProjecttestimonialsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Projecttestimonials
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Projecttestimonials.Include(p => p.User);
            return View(await modelContext.ToListAsync());
        }

        // GET: Projecttestimonials/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Projecttestimonials == null)
            {
                return NotFound();
            }

            var projecttestimonial = await _context.Projecttestimonials
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projecttestimonial == null)
            {
                return NotFound();
            }

            return View(projecttestimonial);
        }

        // GET: Projecttestimonials/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Projectusers, "Id", "Username");
            return View();
        }

        // POST: Projecttestimonials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Satatus,Description,UserId")] Projecttestimonial projecttestimonial)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projecttestimonial);
                await _context.SaveChangesAsync();
                return RedirectToAction("ManageWebsitePages", "Admin");
            }
            ViewData["UserId"] = new SelectList(_context.Projectusers, "Id", "Username", projecttestimonial.UserId);
            return View(projecttestimonial);
        }

        // GET: Projecttestimonials/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Projecttestimonials == null)
            {
                return NotFound();
            }

            var projecttestimonial = await _context.Projecttestimonials.FindAsync(id);
            if (projecttestimonial == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Projectusers, "Id", "Username", projecttestimonial.UserId);
            return View(projecttestimonial);
        }

        // POST: Projecttestimonials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Satatus,Description,UserId")] Projecttestimonial projecttestimonial)
        {
            if (id != projecttestimonial.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projecttestimonial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjecttestimonialExists(projecttestimonial.Id))
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
            ViewData["UserId"] = new SelectList(_context.Projectusers, "Id", "Username", projecttestimonial.UserId);
            return View(projecttestimonial);
        }

        // GET: Projecttestimonials/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Projecttestimonials == null)
            {
                return NotFound();
            }

            var projecttestimonial = await _context.Projecttestimonials
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projecttestimonial == null)
            {
                return NotFound();
            }

            return View(projecttestimonial);
        }

        // POST: Projecttestimonials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Projecttestimonials == null)
            {
                return Problem("Entity set 'ModelContext.Projecttestimonials'  is null.");
            }
            var projecttestimonial = await _context.Projecttestimonials.FindAsync(id);
            if (projecttestimonial != null)
            {
                _context.Projecttestimonials.Remove(projecttestimonial);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("ManageWebsitePages", "Admin");
        }

        private bool ProjecttestimonialExists(decimal id)
        {
          return (_context.Projecttestimonials?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
