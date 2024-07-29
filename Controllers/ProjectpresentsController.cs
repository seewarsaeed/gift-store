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
    public class ProjectpresentsController : Controller
    {
        private readonly ModelContext _context;

        public ProjectpresentsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Projectpresents
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Projectpresents.Include(p => p.Gifts).Include(p => p.User);
            return View(await modelContext.ToListAsync());
        }

        // GET: Projectpresents/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Projectpresents == null)
            {
                return NotFound();
            }

            var projectpresent = await _context.Projectpresents
                .Include(p => p.Gifts)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectpresent == null)
            {
                return NotFound();
            }

            return View(projectpresent);
        }

        // GET: Projectpresents/Create
        public IActionResult Create()
        {
            ViewData["Giftsid"] = new SelectList(_context.Projectgifts, "Id", "Name");
            ViewData["Userid"] = new SelectList(_context.Projectusers, "Id", "Username");
            return View();
        }

        // POST: Projectpresents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Userid,Reciveraddress,Giftsid,Requestdate")] Projectpresent projectpresent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projectpresent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Giftsid"] = new SelectList(_context.Projectgifts, "Id", "Name", projectpresent.Giftsid);
            ViewData["Userid"] = new SelectList(_context.Projectusers, "Id", "Username", projectpresent.Userid);
            return View(projectpresent);
        }

        // GET: Projectpresents/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Projectpresents == null)
            {
                return NotFound();
            }

            var projectpresent = await _context.Projectpresents.FindAsync(id);
            if (projectpresent == null)
            {
                return NotFound();
            }
            ViewData["Giftsid"] = new SelectList(_context.Projectgifts, "Id", "Name", projectpresent.Giftsid);
            ViewData["Userid"] = new SelectList(_context.Projectusers, "Id", "Username", projectpresent.Userid);
            return View(projectpresent);
        }

        // POST: Projectpresents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Userid,Reciveraddress,Giftsid,Requestdate")] Projectpresent projectpresent)
        {
            if (id != projectpresent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectpresent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectpresentExists(projectpresent.Id))
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
            ViewData["Giftsid"] = new SelectList(_context.Projectgifts, "Id", "Name", projectpresent.Giftsid);
            ViewData["Userid"] = new SelectList(_context.Projectusers, "Id", "Username", projectpresent.Userid);
            return View(projectpresent);
        }

        // GET: Projectpresents/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Projectpresents == null)
            {
                return NotFound();
            }

            var projectpresent = await _context.Projectpresents
                .Include(p => p.Gifts)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectpresent == null)
            {
                return NotFound();
            }

            return View(projectpresent);
        }

        // POST: Projectpresents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Projectpresents == null)
            {
                return Problem("Entity set 'ModelContext.Projectpresents'  is null.");
            }
            var projectpresent = await _context.Projectpresents.FindAsync(id);
            if (projectpresent != null)
            {
                _context.Projectpresents.Remove(projectpresent);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectpresentExists(decimal id)
        {
          return (_context.Projectpresents?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
