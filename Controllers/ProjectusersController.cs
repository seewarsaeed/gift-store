using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GiftStore.Models;
using System.Text.RegularExpressions;

namespace GiftStore.Controllers
{
    public class ProjectusersController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProjectusersController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Projectusers.Include(p => p.Role);
            return View(await modelContext.ToListAsync());
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Projectusers == null)
            {
                return NotFound();
            }

            var projectuser = await _context.Projectusers
                .Include(p => p.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectuser == null)
            {
                return NotFound();
            }

            return View(projectuser);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            ViewData["Roleid"] = new SelectList(_context.Projectroles, "Id", "Name");
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ImageFile,Fname,Lname,Email,Pnumber,Address,Status,Username,Password,Roleid")] Projectuser projectuser)
        {
            
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + projectuser.ImageFile.FileName;
                string path = Path.Combine(wwwRootPath + "/Images/UserImages/", fileName);
                using (var filestream = new FileStream(path, FileMode.Create))
                {
                    await projectuser.ImageFile.CopyToAsync(filestream);
                }
                projectuser.Image = fileName;
                    _context.Add(projectuser);
                await _context.SaveChangesAsync();
                if (projectuser.Roleid == 2)
                {
                    return RedirectToAction("Create", "Projectcategoryusers", new { userId = projectuser.Id });
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Roleid"] = new SelectList(_context.Projectroles, "Id", "Name", projectuser.Roleid);
            return View(projectuser);
        }
        // GET: User/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Projectusers == null)
            {
                return NotFound();
            }

            var projectuser = await _context.Projectusers.FindAsync(id);
            if (projectuser == null)
            {
                return NotFound();
            }
            ViewData["Roleid"] = new SelectList(_context.Projectroles, "Id", "Name", projectuser.Roleid);
            return View(projectuser);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,ImageFile,Fname,Lname,Email,Pnumber,Address,Status,Username,Password,Roleid")] Projectuser projectuser)
        {
            if (id != projectuser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (projectuser.ImageFile != null)
                    {
                        string wwwRootPath = _webHostEnvironment.WebRootPath;// Solution root folder
                        string fileName = Guid.NewGuid().ToString() + projectuser.ImageFile.FileName;// encrypt image name
                        string path = Path.Combine(wwwRootPath + "/Images/UserImages/", fileName);// get full path
                        using (var fileStream = new FileStream(path, FileMode.Create))// create the image in determined oath
                        {
                            await projectuser.ImageFile.CopyToAsync(fileStream);
                        }
                        projectuser.Image = fileName;// save the encrypted image name in the DB
                    }

                    _context.Update(projectuser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectuserExists(projectuser.Id))
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
            ViewData["Roleid"] = new SelectList(_context.Projectroles, "Id", "Name", projectuser.Roleid);
            return View(projectuser);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Projectusers == null)
            {
                return NotFound();
            }

            var projectuser = await _context.Projectusers
                .Include(p => p.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectuser == null)
            {
                return NotFound();
            }

            return View(projectuser);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Projectusers == null)
            {
                return Problem("Entity set 'ModelContext.Projectusers'  is null.");
            }
            var projectuser = await _context.Projectusers.FindAsync(id);
            if (projectuser != null)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;// Solution root folder
                string path = Path.Combine(wwwRootPath + "/Images/UserImages/" + projectuser.Image);// get full path
                System.IO.File.Delete(path);

                _context.Projectusers.Remove(projectuser);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectuserExists(decimal id)
        {
            return (_context.Projectusers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
