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
    public class ProjectaboutusController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProjectaboutusController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Projectaboutus
        public async Task<IActionResult> Index()
        {
              return _context.Projectaboutus != null ? 
                          View(await _context.Projectaboutus.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Projectaboutus'  is null.");
        }

        // GET: Projectaboutus/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Projectaboutus == null)
            {
                return NotFound();
            }

            var projectaboutu = await _context.Projectaboutus
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectaboutu == null)
            {
                return NotFound();
            }

            return View(projectaboutu);
        }

        // GET: Projectaboutus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Projectaboutus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,ImageFile")] Projectaboutu projectaboutu)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + projectaboutu.ImageFile.FileName;
                string path = Path.Combine(wwwRootPath + "/Images/AboutImages/", fileName);
                using (var filestream = new FileStream(path, FileMode.Create))
                {
                    await projectaboutu.ImageFile.CopyToAsync(filestream);
                }
                projectaboutu.Image = fileName;

                _context.Add(projectaboutu);
                await _context.SaveChangesAsync();
                return RedirectToAction("ManageWebsitePages", "Admin");
            }
            return View(projectaboutu);
        }

        // GET: Projectaboutus/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Projectaboutus == null)
            {
                return NotFound();
            }

            var projectaboutu = await _context.Projectaboutus.FindAsync(id);
            if (projectaboutu == null)
            {
                return NotFound();
            }
            return View(projectaboutu);
        }

        // POST: Projectaboutus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Description,ImageFile")] Projectaboutu projectaboutu)
        {
            if (id != projectaboutu.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (projectaboutu.ImageFile != null)
                    {
                        string wwwRootPath = _webHostEnvironment.WebRootPath;// Solution root folder
                        string fileName = Guid.NewGuid().ToString() + projectaboutu.ImageFile.FileName;// encrypt image name
                        string path = Path.Combine(wwwRootPath + "/Images/AboutImages/", fileName);// get full path
                        using (var fileStream = new FileStream(path, FileMode.Create))// create the image in determined oath
                        {
                            await projectaboutu.ImageFile.CopyToAsync(fileStream);
                        }
                        projectaboutu.Image = fileName;// save the encrypted image name in the DB
                    }

                    _context.Update(projectaboutu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectaboutuExists(projectaboutu.Id))
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
            return View(projectaboutu);
        }

        // GET: Projectaboutus/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Projectaboutus == null)
            {
                return NotFound();
            }

            var projectaboutu = await _context.Projectaboutus
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectaboutu == null)
            {
                return NotFound();
            }

            return View(projectaboutu);
        }

        // POST: Projectaboutus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Projectaboutus == null)
            {
                return Problem("Entity set 'ModelContext.Projectaboutus'  is null.");
            }
            var projectaboutu = await _context.Projectaboutus.FindAsync(id);
            if (projectaboutu != null)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;// Solution root folder
                string path = Path.Combine(wwwRootPath + "/Images/AboutImages/" + projectaboutu.Image);// get full path
                System.IO.File.Delete(path);

                _context.Projectaboutus.Remove(projectaboutu);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("ManageWebsitePages", "Admin");
        }

        private bool ProjectaboutuExists(decimal id)
        {
          return (_context.Projectaboutus?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
