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
    public class ProjectcategoryController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProjectcategoryController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Projectcategory
        public async Task<IActionResult> Index()
        {
              return _context.Projectcategories != null ? 
                          View(await _context.Projectcategories.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Projectcategories'  is null.");
        }

        // GET: Projectcategory/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Projectcategories == null)
            {
                return NotFound();
            }

            var projectcategory = await _context.Projectcategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectcategory == null)
            {
                return NotFound();
            }

            return View(projectcategory);
        }

        // GET: Projectcategory/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Projectcategory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ImageFile,Name,Description")] Projectcategory projectcategory)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + projectcategory.ImageFile.FileName;
                string path = Path.Combine(wwwRootPath + "/Images/CategoryImages/", fileName);
                using (var filestream = new FileStream(path, FileMode.Create))
                {
                    await projectcategory.ImageFile.CopyToAsync(filestream);
                }
                projectcategory.Image = fileName;

                _context.Add(projectcategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(projectcategory);
        }

        // GET: Projectcategory/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Projectcategories == null)
            {
                return NotFound();
            }

            var projectcategory = await _context.Projectcategories.FindAsync(id);
            if (projectcategory == null)
            {
                return NotFound();
            }
            return View(projectcategory);
        }

        // POST: Projectcategory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,ImageFile,Name,Description")] Projectcategory projectcategory)
        {
            if (id != projectcategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (projectcategory.ImageFile != null)
                    {
                        string wwwRootPath = _webHostEnvironment.WebRootPath;// Solution root folder
                        string fileName = Guid.NewGuid().ToString() + projectcategory.ImageFile.FileName;// encrypt image name
                        string path = Path.Combine(wwwRootPath + "/Images/CategoryImages/", fileName);// get full path
                        using (var fileStream = new FileStream(path, FileMode.Create))// create the image in determined oath
                        {
                            await projectcategory.ImageFile.CopyToAsync(fileStream);
                        }
                        projectcategory.Image = fileName;// save the encrypted image name in the DB
                    }

                    _context.Update(projectcategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectcategoryExists(projectcategory.Id))
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
            return View(projectcategory);
        }

        // GET: Projectcategory/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Projectcategories == null)
            {
                return NotFound();
            }

            var projectcategory = await _context.Projectcategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectcategory == null)
            {
                return NotFound();
            }

            return View(projectcategory);
        }

        // POST: Projectcategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Projectcategories == null)
            {
                return Problem("Entity set 'ModelContext.Projectcategories'  is null.");
            }
            var projectcategory = await _context.Projectcategories.FindAsync(id);
            if (projectcategory != null)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;// Solution root folder
                string path = Path.Combine(wwwRootPath + "/Images/CategoryImages/" + projectcategory.Image);// get full path
                System.IO.File.Delete(path);

                _context.Projectcategories.Remove(projectcategory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectcategoryExists(decimal id)
        {
          return (_context.Projectcategories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
