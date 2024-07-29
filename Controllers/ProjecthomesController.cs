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
    public class ProjecthomesController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProjecthomesController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Projecthomes
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Projecthomes.Include(p => p.Aboutus).Include(p => p.Background).Include(p => p.Contactus).Include(p => p.Footer).Include(p => p.Header).Include(p => p.Testimonial);
            return View(await modelContext.ToListAsync());
        }

        // GET: Projecthomes/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Projecthomes == null)
            {
                return NotFound();
            }

            var projecthome = await _context.Projecthomes
                .Include(p => p.Aboutus)
                .Include(p => p.Background)
                .Include(p => p.Contactus)
                .Include(p => p.Footer)
                .Include(p => p.Header)
                .Include(p => p.Testimonial)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projecthome == null)
            {
                return NotFound();
            }

            return View(projecthome);
        }

        // GET: Projecthomes/Create
        public IActionResult Create()
        {
            ViewData["Aboutusid"] = new SelectList(_context.Projectaboutus, "Id", "Id");
            ViewData["Backgroundid"] = new SelectList(_context.Projectbackgrounds, "Id", "Id");
            ViewData["Contactusid"] = new SelectList(_context.Projectcontactus, "Id", "Id");
            ViewData["Footerid"] = new SelectList(_context.Projectfooters, "Id", "Id");
            ViewData["Headerid"] = new SelectList(_context.Projectheaders, "Id", "Id");
            ViewData["Testimonialid"] = new SelectList(_context.Projecttestimonials, "Id", "Id");
            return View();
        }

        // POST: Projecthomes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ImageFile,Name,Backgroundid,Contactusid,Headerid,Aboutusid,Footerid,Testimonialid")] Projecthome projecthome)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + projecthome.ImageFile.FileName;
                string path = Path.Combine(wwwRootPath + "/Images/Logo/", fileName);
                using (var filestream = new FileStream(path, FileMode.Create))
                {
                    await projecthome.ImageFile.CopyToAsync(filestream);
                }
                projecthome.Logo = fileName;


                _context.Add(projecthome);
                await _context.SaveChangesAsync();
                return RedirectToAction("ManageWebsitePages", "Admin");
            }
            ViewData["Aboutusid"] = new SelectList(_context.Projectaboutus, "Id", "Id", projecthome.Aboutusid);
            ViewData["Backgroundid"] = new SelectList(_context.Projectbackgrounds, "Id", "Id", projecthome.Backgroundid);
            ViewData["Contactusid"] = new SelectList(_context.Projectcontactus, "Id", "Id", projecthome.Contactusid);
            ViewData["Footerid"] = new SelectList(_context.Projectfooters, "Id", "Id", projecthome.Footerid);
            ViewData["Headerid"] = new SelectList(_context.Projectheaders, "Id", "Id", projecthome.Headerid);
            ViewData["Testimonialid"] = new SelectList(_context.Projecttestimonials, "Id", "Id", projecthome.Testimonialid);
            return View(projecthome);
        }

        // GET: Projecthomes/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Projecthomes == null)
            {
                return NotFound();
            }

            var projecthome = await _context.Projecthomes.FindAsync(id);
            if (projecthome == null)
            {
                return NotFound();
            }
            ViewData["Aboutusid"] = new SelectList(_context.Projectaboutus, "Id", "Id", projecthome.Aboutusid);
            ViewData["Backgroundid"] = new SelectList(_context.Projectbackgrounds, "Id", "Id", projecthome.Backgroundid);
            ViewData["Contactusid"] = new SelectList(_context.Projectcontactus, "Id", "Id", projecthome.Contactusid);
            ViewData["Footerid"] = new SelectList(_context.Projectfooters, "Id", "Id", projecthome.Footerid);
            ViewData["Headerid"] = new SelectList(_context.Projectheaders, "Id", "Id", projecthome.Headerid);
            ViewData["Testimonialid"] = new SelectList(_context.Projecttestimonials, "Id", "Id", projecthome.Testimonialid);
            return View(projecthome);
        }

        // POST: Projecthomes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,ImageFile,Name,Backgroundid,Contactusid,Headerid,Aboutusid,Footerid,Testimonialid")] Projecthome projecthome)
        {
            if (id != projecthome.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (projecthome.ImageFile != null)
                    {
                        string wwwRootPath = _webHostEnvironment.WebRootPath;// Solution root folder
                        string fileName = Guid.NewGuid().ToString() + projecthome.ImageFile.FileName;// encrypt image name
                        string path = Path.Combine(wwwRootPath + "/Images/Logo/", fileName);// get full path
                        using (var fileStream = new FileStream(path, FileMode.Create))// create the image in determined oath
                        {
                            await projecthome.ImageFile.CopyToAsync(fileStream);
                        }
                        projecthome.Logo = fileName;// save the encrypted image name in the DB
                    }

                    _context.Update(projecthome);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjecthomeExists(projecthome.Id))
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
            ViewData["Aboutusid"] = new SelectList(_context.Projectaboutus, "Id", "Id", projecthome.Aboutusid);
            ViewData["Backgroundid"] = new SelectList(_context.Projectbackgrounds, "Id", "Id", projecthome.Backgroundid);
            ViewData["Contactusid"] = new SelectList(_context.Projectcontactus, "Id", "Id", projecthome.Contactusid);
            ViewData["Footerid"] = new SelectList(_context.Projectfooters, "Id", "Id", projecthome.Footerid);
            ViewData["Headerid"] = new SelectList(_context.Projectheaders, "Id", "Id", projecthome.Headerid);
            ViewData["Testimonialid"] = new SelectList(_context.Projecttestimonials, "Id", "Id", projecthome.Testimonialid);
            return View(projecthome);
        }

        // GET: Projecthomes/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Projecthomes == null)
            {
                return NotFound();
            }

            var projecthome = await _context.Projecthomes
                .Include(p => p.Aboutus)
                .Include(p => p.Background)
                .Include(p => p.Contactus)
                .Include(p => p.Footer)
                .Include(p => p.Header)
                .Include(p => p.Testimonial)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projecthome == null)
            {
                return NotFound();
            }

            return View(projecthome);
        }

        // POST: Projecthomes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Projecthomes == null)
            {
                return Problem("Entity set 'ModelContext.Projecthomes'  is null.");
            }
            var projecthome = await _context.Projecthomes.FindAsync(id);
            if (projecthome != null)
            {
                _context.Projecthomes.Remove(projecthome);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("ManageWebsitePages", "Admin");
        }

        private bool ProjecthomeExists(decimal id)
        {
          return (_context.Projecthomes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
