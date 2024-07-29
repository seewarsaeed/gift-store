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
    public class ProjectpaymentsController : Controller
    {
        private readonly ModelContext _context;

        public ProjectpaymentsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Projectpayments
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Projectpayments.Include(p => p.User);
            return View(await modelContext.ToListAsync());
        }

        // GET: Projectpayments/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Projectpayments == null)
            {
                return NotFound();
            }

            var projectpayment = await _context.Projectpayments
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectpayment == null)
            {
                return NotFound();
            }

            return View(projectpayment);
        }

        // GET: Projectpayments/Create
        public IActionResult Create()
        {
            ViewData["Userid"] = new SelectList(_context.Projectusers, "Id", "Username");
            return View();
        }

        // POST: Projectpayments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Cardnumber,Expirationdate,Cvvcvc,Availablebalance,Userid")] Projectpayment projectpayment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projectpayment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Userid"] = new SelectList(_context.Projectusers, "Id", "Username", projectpayment.Userid);
            return View(projectpayment);
        }

        // GET: Projectpayments/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Projectpayments == null)
            {
                return NotFound();
            }

            var projectpayment = await _context.Projectpayments.FindAsync(id);
            if (projectpayment == null)
            {
                return NotFound();
            }
            ViewData["Userid"] = new SelectList(_context.Projectusers, "Id", "Username", projectpayment.Userid);
            return View(projectpayment);
        }

        // POST: Projectpayments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Cardnumber,Expirationdate,Cvvcvc,Availablebalance,Userid")] Projectpayment projectpayment)
        {
            if (id != projectpayment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectpayment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectpaymentExists(projectpayment.Id))
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
            ViewData["Userid"] = new SelectList(_context.Projectusers, "Id", "Username", projectpayment.Userid);
            return View(projectpayment);
        }

        // GET: Projectpayments/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Projectpayments == null)
            {
                return NotFound();
            }

            var projectpayment = await _context.Projectpayments
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectpayment == null)
            {
                return NotFound();
            }

            return View(projectpayment);
        }

        // POST: Projectpayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Projectpayments == null)
            {
                return Problem("Entity set 'ModelContext.Projectpayments'  is null.");
            }
            var projectpayment = await _context.Projectpayments.FindAsync(id);
            if (projectpayment != null)
            {
                _context.Projectpayments.Remove(projectpayment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectpaymentExists(decimal id)
        {
          return (_context.Projectpayments?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
