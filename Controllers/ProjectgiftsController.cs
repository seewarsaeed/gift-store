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
    public class ProjectgiftsController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProjectgiftsController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Projectgifts
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Projectgifts.Include(p => p.CategoryUsr).Include(p => p.User);
            return View(await modelContext.ToListAsync());
        }

        // GET: Projectgifts/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Projectgifts == null)
            {
                return NotFound();
            }

            var projectgift = await _context.Projectgifts
                .Include(p => p.CategoryUsr)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectgift == null)
            {
                return NotFound();
            }

            return View(projectgift);
        }

        // GET: Projectgifts/Create
        public IActionResult Create()
        {
            ViewData["CategoryUsrId"] = new SelectList(_context.Projectcategoryusers, "Id", "Id");
            ViewData["Userid"] = new SelectList(_context.Projectusers, "Id", "Username");
            return View();
        }

        // POST: Projectgifts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ImageFile,Name,Description,CategoryUsrId,Price")] Projectgift projectgift)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + projectgift.ImageFile.FileName;
                string path = Path.Combine(wwwRootPath + "/Images/GiftImages/", fileName);
                using (var filestream = new FileStream(path, FileMode.Create))
                {
                    await projectgift.ImageFile.CopyToAsync(filestream);
                }
                projectgift.Image = fileName;

                _context.Add(projectgift);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryUsrId"] = new SelectList(_context.Projectcategoryusers, "Id", "Id", projectgift.CategoryUsrId);
            ViewData["Userid"] = new SelectList(_context.Projectusers, "Id", "Username", projectgift.Userid);
            return View(projectgift);
        }

        // GET: Projectgifts/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Projectgifts == null)
            {
                return NotFound();
            }

            var projectgift = await _context.Projectgifts.FindAsync(id);
            if (projectgift == null)
            {
                return NotFound();
            }
            ViewData["CategoryUsrId"] = new SelectList(_context.Projectcategoryusers, "Id", "Id", projectgift.CategoryUsrId);
            ViewData["Userid"] = new SelectList(_context.Projectusers, "Id", "Username", projectgift.Userid);
            return View(projectgift);
        }

        // POST: Projectgifts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,ImageFile,Name,Description,CategoryUsrId,Price,Userid")] Projectgift projectgift)
        {
            if (id != projectgift.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (projectgift.ImageFile != null)
                    {
                        string wwwRootPath = _webHostEnvironment.WebRootPath;// Solution root folder
                        string fileName = Guid.NewGuid().ToString() + projectgift.ImageFile.FileName;// encrypt image name
                        string path = Path.Combine(wwwRootPath + "/Images/GiftImages/", fileName);// get full path
                        using (var fileStream = new FileStream(path, FileMode.Create))// create the image in determined oath
                        {
                            await projectgift.ImageFile.CopyToAsync(fileStream);
                        }
                        projectgift.Image = fileName;// save the encrypted image name in the DB
                    }
                    _context.Update(projectgift);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectgiftExists(projectgift.Id))
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
            ViewData["CategoryUsrId"] = new SelectList(_context.Projectcategoryusers, "Id", "Id", projectgift.CategoryUsrId);
            ViewData["Userid"] = new SelectList(_context.Projectusers, "Id", "Username", projectgift.Userid);
            return View(projectgift);
        }

        // GET: Projectgifts/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Projectgifts == null)
            {
                return NotFound();
            }

            var projectgift = await _context.Projectgifts
                .Include(p => p.CategoryUsr)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectgift == null)
            {
                return NotFound();
            }

            return View(projectgift);
        }

        // POST: Projectgifts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Projectgifts == null)
            {
                return Problem("Entity set 'ModelContext.Projectgifts'  is null.");
            }
            var projectgift = await _context.Projectgifts.FindAsync(id);
            if (projectgift != null)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;// Solution root folder
                string path = Path.Combine(wwwRootPath + "/Images/GiftImages/" + projectgift.Image);// get full path
                System.IO.File.Delete(path);

                _context.Projectgifts.Remove(projectgift);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectgiftExists(decimal id)
        {
          return (_context.Projectgifts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
