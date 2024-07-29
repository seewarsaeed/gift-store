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
    public class ProjectcategoryusersController : Controller
    {
        private readonly ModelContext _context;

        public ProjectcategoryusersController(ModelContext context)
        {
            _context = context;
        }

        // GET: Projectcategoryusers
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Projectcategoryusers.Include(p => p.Category).Include(p => p.User);
            return View(await modelContext.ToListAsync());
        }

        // GET: Projectcategoryusers/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Projectcategoryusers == null)
            {
                return NotFound();
            }

            var projectcategoryuser = await _context.Projectcategoryusers
                .Include(p => p.Category)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectcategoryuser == null)
            {
                return NotFound();
            }

            return View(projectcategoryuser);
        }

        // GET: Projectcategoryusers/Create
        public IActionResult Create(int? userId)
        {
            if (userId == null)
            {
                return RedirectToAction("Error");
            }

            var user = _context.Projectusers.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return RedirectToAction("Error");
            }

            ViewData["Categoryid"] = new SelectList(_context.Projectcategories, "Id", "Name");
            ViewData["Userid"] = new SelectList(new List<Projectuser> { user }, "Id", "Username");
            return View();
        }

        // POST: Projectcategoryusers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Userid,Categoryid")] Projectcategoryuser projectcategoryuser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projectcategoryuser);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Projectusers");
            }
            ViewData["Categoryid"] = new SelectList(_context.Projectcategories, "Id", "Name", projectcategoryuser.Categoryid);
            ViewData["Userid"] = new SelectList(_context.Projectusers, "Id", "Username", projectcategoryuser.Userid);
            return View(projectcategoryuser);
        }

        // GET: Projectcategoryusers/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Projectcategoryusers == null)
            {
                return NotFound();
            }

            var projectcategoryuser = await _context.Projectcategoryusers.FindAsync(id);
            if (projectcategoryuser == null)
            {
                return NotFound();
            }
            ViewData["Categoryid"] = new SelectList(_context.Projectcategories, "Id", "Name", projectcategoryuser.Categoryid);
            ViewData["Userid"] = new SelectList(_context.Projectusers, "Id", "Username", projectcategoryuser.Userid);
            return View(projectcategoryuser);
        }

        // POST: Projectcategoryusers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Userid,Categoryid")] Projectcategoryuser projectcategoryuser)
        {
            if (id != projectcategoryuser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectcategoryuser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectcategoryuserExists(projectcategoryuser.Id))
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
            ViewData["Categoryid"] = new SelectList(_context.Projectcategories, "Id", "Name", projectcategoryuser.Categoryid);
            ViewData["Userid"] = new SelectList(_context.Projectusers, "Id", "Username", projectcategoryuser.Userid);
            return View(projectcategoryuser);
        }

        // GET: Projectcategoryusers/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Projectcategoryusers == null)
            {
                return NotFound();
            }

            var projectcategoryuser = await _context.Projectcategoryusers
                .Include(p => p.Category)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectcategoryuser == null)
            {
                return NotFound();
            }

            return View(projectcategoryuser);
        }

        // POST: Projectcategoryusers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Projectcategoryusers == null)
            {
                return Problem("Entity set 'ModelContext.Projectcategoryusers'  is null.");
            }
            var projectcategoryuser = await _context.Projectcategoryusers.FindAsync(id);
            if (projectcategoryuser != null)
            {
                _context.Projectcategoryusers.Remove(projectcategoryuser);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectcategoryuserExists(decimal id)
        {
          return (_context.Projectcategoryusers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
