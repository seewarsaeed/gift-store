using GiftStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;
using System.Text.RegularExpressions;
using BCrypt.Net;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace GiftStore.Controllers
{
    public class AuthController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ModelContext _context;
        public AuthController(IWebHostEnvironment webHostEnvironment, ModelContext context)
        {
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        public IActionResult Login()
        {
                var Home = _context.Projecthomes
               .Include(p => p.Background)
               .Include(p => p.Footer)
               .Include(p => p.Header)
               .Include(p => p.Contactus)
               .Include(p => p.Aboutus)
               .Where(p => p.Name == "Guest").FirstOrDefault();
                ViewData["Home"] = Home;
            return View();

        }
        [HttpPost]
        public IActionResult Login([Bind("Username,Password")] Projectuser projectuser)
        {
            var auth = _context.Projectusers.Where(x => x.Username == projectuser.Username && x.Password == projectuser.Password).SingleOrDefault();

            if (auth != null && auth.Status?.ToString() == "Available")
            {
                string newIdString = auth.Id.ToString();
                switch (auth.Roleid)
                {
                    case 2:
                        HttpContext.Session.SetString("userId", newIdString);
                        return RedirectToAction("Index", "GiftMaker");
                    case 1:
                        HttpContext.Session.SetString("userId", newIdString);
                        HttpContext.Session.SetString("username", auth.Username);
                        return RedirectToAction("Index", "Admin");
                    case 3:
                        HttpContext.Session.SetString("userId", newIdString);
                        HttpContext.Session.SetString("username", auth.Username);
                        return RedirectToAction("Index", "GiftSender");
                }

            }
            ModelState.AddModelError(string.Empty, "Invalid username or password, Try again please"); // Add error message to ModelState
            var Home = _context.Projecthomes
                       .Include(p => p.Background)
                       .Include(p => p.Footer)
                       .Include(p => p.Header)
                       .Include(p => p.Contactus)
                       .Include(p => p.Aboutus)
                       .Where(p => p.Name == "Guest").FirstOrDefault();
            ViewData["Home"] = Home;
            return View();


        }

        public IActionResult Signup()
        {
            List<Projectcategory> categories = _context.Projectcategories.ToList();
            var Home = _context.Projecthomes
                       .Include(p => p.Background)
                       .Include(p => p.Footer)
                       .Include(p => p.Header)
                       .Include(p => p.Contactus)
                       .Include(p => p.Aboutus)
                       .Where(p => p.Name == "Guest").FirstOrDefault();
            ViewData["Home"] = Home;
            return View(categories);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Signup(string Fname, string Lname, string Email, string Address, string Pnumber, IFormFile ImageFile, string Username, string Password, bool selector, int category)
        {
            List<Projectcategory> categories = _context.Projectcategories.ToList();
            var Home = _context.Projecthomes
                       .Include(p => p.Background)
                       .Include(p => p.Footer)
                       .Include(p => p.Header)
                       .Include(p => p.Contactus)
                       .Include(p => p.Aboutus)
                       .Where(p => p.Name == "Guest").FirstOrDefault();
            ViewData["Home"] = Home;
            if (string.IsNullOrEmpty(Fname) || string.IsNullOrEmpty(Lname) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Address) || string.IsNullOrEmpty(Pnumber) || string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password) || ImageFile == null)
            {
                ModelState.AddModelError(string.Empty, "You should enter all data please"); // Add error message to ModelState
                
                return View(categories);
            }

            else
            {
                bool usernameExists = await _context.Projectusers.AnyAsync(l => l.Username == Username);
                if (usernameExists)
                {
                    ModelState.AddModelError(string.Empty, "This username is already exist try again."); // Add error message to ModelState

                    return View(categories);
                }
                else if (!IsStrongPassword(Password))
                {
                    ModelState.AddModelError(string.Empty, "Password should be at least 8 characters long and contain a combination of uppercase, lowercase, numeric, and special characters."); // Add error message to ModelState
                    return View(categories);
                }
                else if (!IsValidMobileNumber(Pnumber))
                {
                    ModelState.AddModelError(string.Empty, "Mobile number should be a valid Jordanian number, please try again"); // Add error message to ModelState
                    return View(categories);
                }
                string status = selector ? "Pending" : "Available";
                int roleId = selector ? 2 : 3;
                var projectuser = new Projectuser
                {
                    Fname = Fname,
                    Lname = Lname,
                    Email = Email,
                    Address = Address,
                    Pnumber = Pnumber,
                    Username = Username,
                    Password = Password,
                    Roleid = roleId,
                    Status = status,
                    ImageFile = ImageFile
                };

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
                    var projectcategoryuser = new Projectcategoryuser
                    {
                        Userid = projectuser.Id,
                        Categoryid = category
                    };
                    _context.Add(projectcategoryuser);
                    await _context.SaveChangesAsync();
                    ViewData["SuccessMessage"] = "Signup successful, but we need to review your request and we will send the update through email.";
                    return View(categories);
                }
                ViewData["SuccessMessage"] = "Signup successful, You can now make the people you love happy.";
                return View(categories);
            }
        }
        private bool IsStrongPassword(string password)
        {
            return password.Length >= 8 && Regex.IsMatch(password, @"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[@#$%^&+=!]).*$");
        }
        private bool IsValidMobileNumber(string pnumber)
        {
            return Regex.IsMatch(pnumber, @"^07[789]\d{7}$");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Clear all session data
            return RedirectToAction("Login", "Auth");
        }
    }
}
