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
    public class ProjectbackgroundsController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProjectbackgroundsController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Projectbackgrounds
        public async Task<IActionResult> Index()
        {
              return _context.Projectbackgrounds != null ? 
                          View(await _context.Projectbackgrounds.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Projectbackgrounds'  is null.");
        }

        // GET: Projectbackgrounds/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Projectbackgrounds == null)
            {
                return NotFound();
            }

            var projectbackground = await _context.Projectbackgrounds
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectbackground == null)
            {
                return NotFound();
            }

            return View(projectbackground);
        }

        // GET: Projectbackgrounds/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Projectbackgrounds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ImageFile,Description")] Projectbackground projectbackground)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + projectbackground.ImageFile.FileName;
                string path = Path.Combine(wwwRootPath + "/Images/BackgroundImages/", fileName);
                using (var filestream = new FileStream(path, FileMode.Create))
                {
                    await projectbackground.ImageFile.CopyToAsync(filestream);
                }
                projectbackground.Image = fileName;


                _context.Add(projectbackground);
                await _context.SaveChangesAsync();
                return RedirectToAction("ManageWebsitePages", "Admin");
            }
            return View(projectbackground);
        }

        // GET: Projectbackgrounds/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Projectbackgrounds == null)
            {
                return NotFound();
            }

            var projectbackground = await _context.Projectbackgrounds.FindAsync(id);
            if (projectbackground == null)
            {
                return NotFound();
            }
            return View(projectbackground);
        }

        // POST: Projectbackgrounds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,ImageFile,Description")] Projectbackground projectbackground)
        {
            if (id != projectbackground.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (projectbackground.ImageFile != null)
                    {
                        string wwwRootPath = _webHostEnvironment.WebRootPath;// Solution root folder
                        string fileName = Guid.NewGuid().ToString() + projectbackground.ImageFile.FileName;// encrypt image name
                        string path = Path.Combine(wwwRootPath + "/Images/BackgroundImages/", fileName);// get full path
                        using (var fileStream = new FileStream(path, FileMode.Create))// create the image in determined oath
                        {
                            await projectbackground.ImageFile.CopyToAsync(fileStream);
                        }
                        projectbackground.Image = fileName;// save the encrypted image name in the DB
                    }

                    _context.Update(projectbackground);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectbackgroundExists(projectbackground.Id))
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
            return View(projectbackground);
        }

        // GET: Projectbackgrounds/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Projectbackgrounds == null)
            {
                return NotFound();
            }

            var projectbackground = await _context.Projectbackgrounds
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectbackground == null)
            {
                return NotFound();
            }

            return View(projectbackground);
        }

        // POST: Projectbackgrounds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Projectbackgrounds == null)
            {
                return Problem("Entity set 'ModelContext.Projectbackgrounds'  is null.");
            }
            var projectbackground = await _context.Projectbackgrounds.FindAsync(id);
            if (projectbackground != null)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;// Solution root folder
                string path = Path.Combine(wwwRootPath + "/Images/BackgroundImages/" + projectbackground.Image);// get full path
                System.IO.File.Delete(path);

                _context.Projectbackgrounds.Remove(projectbackground);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("ManageWebsitePages", "Admin");
        }

        private bool ProjectbackgroundExists(decimal id)
        {
          return (_context.Projectbackgrounds?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
