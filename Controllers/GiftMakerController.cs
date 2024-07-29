using GiftStore.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Threading.Tasks;
using System.Security.Permissions;

namespace GiftStore.Controllers
{
    public class GiftMakerController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<HomeController> _logger;

        public GiftMakerController(ModelContext context, IWebHostEnvironment webHostEnvironment, ILogger<HomeController> logger)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }
        SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587)
        {
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential("gifthavenjo@gmail.com", "rjoahehjvocuiose"),
            EnableSsl = true
        };

        public IActionResult Index()
        {
            decimal userId = Convert.ToDecimal(HttpContext.Session.GetString("userId"));
            int pendingPresentsCount = _context.Projectpresents
                .Where(p => p.Gifts.CategoryUsr.Userid == userId)
                .Count(p => p.Projectstatuses.Any(s => s.Notifecationstatus == "pending"));
            ViewData["Notification"] = pendingPresentsCount;
            var Home = _context.Projecthomes
                    .Include(p => p.Background)
                    .Include(p => p.Footer)
                    .Include(p => p.Header)
                    .Include(p => p.Contactus)
                    .Include(p => p.Aboutus)
                    .Where(p => p.Name == "Gift Maker").FirstOrDefault();
            ViewData["Home"] = Home;
            var Category = _context.Projectcategoryusers
                .Where(g => g.Userid == userId)
                .Include(g => g.Category).FirstOrDefault();
            ViewData["Category"] = Category;
            var Testimonial = _context.Projecttestimonials.Include(p => p.User).Where(p => p.Satatus == "Accept").ToList();
            ViewData["Testimonial"] = Testimonial;
            return View();
        }
        public IActionResult Gifts()
        {
            decimal userId = Convert.ToDecimal(HttpContext.Session.GetString("userId"));
            int pendingPresentsCount = _context.Projectpresents
                .Where(p => p.Gifts.CategoryUsr.Userid == userId)
                .Count(p => p.Projectstatuses.Any(s => s.Notifecationstatus == "pending"));
            ViewData["Notification"] = pendingPresentsCount;
            var Home = _context.Projecthomes
                        .Include(p => p.Background)
                        .Include(p => p.Footer)
                        .Include(p => p.Header)
                        .Include(p => p.Contactus)
                        .Include(p => p.Aboutus)
                        .Where(p => p.Name == "Gift Maker").FirstOrDefault();
            ViewData["Home"] = Home;
            string ID = HttpContext.Session.GetString("userId");
            if (ID == null)
            {
                return NotFound();
            }
            var gifts = _context.Projectgifts.Include(p => p.CategoryUsr)
                                    .ThenInclude(cu => cu.Category)
                                .Include(p => p.CategoryUsr)
                                    .ThenInclude(cu => cu.User).Where(cu => cu.CategoryUsr.Userid.ToString() == ID).ToList();

            var categoryName = _context.Projectgifts
                                .Include(p => p.CategoryUsr)
                                    .ThenInclude(cu => cu.User)
                                .FirstOrDefault(g => g.CategoryUsr.Userid.ToString() == ID);

            ViewData["categoryName"] = categoryName?.CategoryUsr?.Category?.Name;

            return View(gifts);
        }
        public async Task<IActionResult> Item(decimal? id)
        {
            decimal userId = Convert.ToDecimal(HttpContext.Session.GetString("userId"));
            int pendingPresentsCount = _context.Projectpresents
                .Where(p => p.Gifts.CategoryUsr.Userid == userId)
                .Count(p => p.Projectstatuses.Any(s => s.Notifecationstatus == "pending"));
            ViewData["Notification"] = pendingPresentsCount;
            var Home = _context.Projecthomes
                    .Include(p => p.Background)
                    .Include(p => p.Footer)
                    .Include(p => p.Header)
                    .Include(p => p.Contactus)
                    .Include(p => p.Aboutus)
                    .Where(p => p.Name == "Gift Maker").FirstOrDefault();
            ViewData["Home"] = Home;
            if (id == null)
            {
                return NotFound();
            }
            var gift = await _context.Projectgifts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gift == null)
            {
                return NotFound();
            }
            return View(gift);
        }
        public IActionResult CreateGifts()
        {
            decimal userId = Convert.ToDecimal(HttpContext.Session.GetString("userId"));
            int pendingPresentsCount = _context.Projectpresents
                .Where(p => p.Gifts.CategoryUsr.Userid == userId)
                .Count(p => p.Projectstatuses.Any(s => s.Notifecationstatus == "pending"));
            ViewData["Notification"] = pendingPresentsCount;
            var Home = _context.Projecthomes
                        .Include(p => p.Background)
                        .Include(p => p.Footer)
                        .Include(p => p.Header)
                        .Include(p => p.Contactus)
                        .Include(p => p.Aboutus)
                        .Where(p => p.Name == "Gift Maker").FirstOrDefault();
            ViewData["Home"] = Home;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateGifts([Bind("Id,ImageFile,Name,Description,Price")] Projectgift projectgift)
        {
            string ID = HttpContext.Session.GetString("userId");
            var Category = _context.Projectcategoryusers
                .Where(g => g.Userid.ToString() == ID).FirstOrDefault();

            if (ID == null)
            {
                return NotFound();
            }
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
                projectgift.CategoryUsrId = Category.Id;
                _context.Add(projectgift);
                await _context.SaveChangesAsync();
                return RedirectToAction("Gifts");
            }

            return View();
        }
        public IActionResult Profile()
        {
            var Home = _context.Projecthomes
                        .Include(p => p.Background)
                        .Include(p => p.Footer)
                        .Include(p => p.Header)
                        .Include(p => p.Contactus)
                        .Include(p => p.Aboutus)
                        .Where(p => p.Name == "Gift Maker").FirstOrDefault();
            ViewData["Home"] = Home;

            decimal userId = Convert.ToDecimal(HttpContext.Session.GetString("userId"));

            int pendingPresentsCount = _context.Projectpresents
                .Where(p => p.Gifts.CategoryUsr.Userid == userId)
                .Count(p => p.Projectstatuses.Any(s => s.Notifecationstatus == "pending"));
            ViewData["Notification"] = pendingPresentsCount;

            string ID = HttpContext.Session.GetString("userId");
            var user = _context.Projectusers.Where(p => p.Id.ToString() == ID).SingleOrDefault();
            ViewData["user"] = user;
            var categoryName = _context.Projectcategoryusers
                .Where(g => g.Userid.ToString() == ID)
                .Select(g => g.Category.Name)
                .FirstOrDefault();
                        ViewData["categoryName"] = categoryName;
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(IFormFile ImageFile, string username, string Fname, string Lname, string email, string pnumber, string Address, string oldPassword, string NewPassword, string reNewPassword)
        {
            var Home = _context.Projecthomes
                        .Include(p => p.Background)
                        .Include(p => p.Footer)
                        .Include(p => p.Header)
                        .Include(p => p.Contactus)
                        .Include(p => p.Aboutus)
                        .Where(p => p.Name == "Gift Maker").FirstOrDefault();
            ViewData["Home"] = Home;
            decimal userId = Convert.ToDecimal(HttpContext.Session.GetString("userId"));
            int pendingPresentsCount = _context.Projectpresents
                .Where(p => p.Gifts.CategoryUsr.Userid == userId)
                .Count(p => p.Projectstatuses.Any(s => s.Notifecationstatus == "pending"));
            ViewData["Notification"] = pendingPresentsCount;

            ViewBag.username = HttpContext.Session.GetString("username");

            string ID = HttpContext.Session.GetString("userId");
            var user = _context.Projectusers.Where(p => p.Id.ToString() == ID).SingleOrDefault();

            var categoryName = _context.Projectcategoryusers
                .Where(g => g.Userid.ToString() == ID)
                .Select(g => g.Category.Name)
                .FirstOrDefault();
            ViewData["user"] = user;
            ViewData["categoryName"] = categoryName;

            if (user != null && user.Password == oldPassword)
            {

                if (!string.IsNullOrEmpty(Fname))
                {
                    user.Fname = Fname;
                }
                if (!string.IsNullOrEmpty(Lname))
                {
                    user.Lname = Lname;
                }
                if (!string.IsNullOrEmpty(email))
                {
                    user.Email = email;
                }
                if (!string.IsNullOrEmpty(pnumber))
                {
                    if (IsValidMobileNumber(pnumber))
                        user.Pnumber = pnumber;
                    else
                    {
                        ViewData["Error"] = "Mobile number should be a valid Jordanian number, please try again";
                        return View();
                    }
                }
                if (!string.IsNullOrEmpty(Address))
                {
                    user.Address = Address;
                }
                if (ImageFile != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + "_" + ImageFile.FileName;
                    string path = Path.Combine(wwwRootPath + "/Images/UserImages/", fileName);
                    using (var filestream = new FileStream(path, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(filestream);
                    }

                    user.Image = fileName;

                }
                if (!string.IsNullOrEmpty(NewPassword) && !string.IsNullOrEmpty(reNewPassword))
                {
                    if (NewPassword == reNewPassword)
                    {
                        if (IsStrongPassword(NewPassword))
                            user.Password = NewPassword;
                        else
                        {
                            ViewData["Error"] = "Password should be at least 8 characters long and contain a combination of uppercase, lowercase, numeric, and special characters.";
                            return View();
                        }
                    }
                    else
                    {
                        ViewData["Error"] = "Passwords are Not Matching, try again";
                        return View();
                    }
                }
                _context.SaveChanges();
                ViewData["Success"] = "Your Profile Edites Successfully";

                ViewData["user"] = user;
                return View();

            }
            ViewData["Error"] = "You should enter your current password correctly, try again please.";
            return View();
        }
        private bool IsStrongPassword(string password)
        {
            return password.Length >= 8 && Regex.IsMatch(password, @"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[@#$%^&+=!]).*$");
        }
        private bool IsValidMobileNumber(string pnumber)
        {
            return Regex.IsMatch(pnumber, @"^07[789]\d{7}$");
        }
        public async Task<IActionResult> Delete(decimal? id)
        {
            var Home = _context.Projecthomes
                    .Include(p => p.Background)
                    .Include(p => p.Footer)
                    .Include(p => p.Header)
                    .Include(p => p.Contactus)
                    .Include(p => p.Aboutus)
                    .Where(p => p.Name == "Gift Maker").FirstOrDefault();
            ViewData["Home"] = Home;
            decimal userId = Convert.ToDecimal(HttpContext.Session.GetString("userId"));
            int pendingPresentsCount = _context.Projectpresents
                .Where(p => p.Gifts.CategoryUsr.Userid == userId)
                .Count(p => p.Projectstatuses.Any(s => s.Notifecationstatus == "pending"));
            ViewData["Notification"] = pendingPresentsCount;

            if (id == null)
            {
                return NotFound();
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

            return RedirectToAction(nameof(Gifts));
        }
        public IActionResult EditItem(decimal? id)
        {
            var Home = _context.Projecthomes
                    .Include(p => p.Background)
                    .Include(p => p.Footer)
                    .Include(p => p.Header)
                    .Include(p => p.Contactus)
                    .Include(p => p.Aboutus)
                    .Where(p => p.Name == "Gift Maker").FirstOrDefault();
            ViewData["Home"] = Home;

            decimal userId = Convert.ToDecimal(HttpContext.Session.GetString("userId"));
            int pendingPresentsCount = _context.Projectpresents
                .Where(p => p.Gifts.CategoryUsr.Userid == userId)
                .Count(p => p.Projectstatuses.Any(s => s.Notifecationstatus == "pending"));
            ViewData["Notification"] = pendingPresentsCount;

            if (id == null)
            {
                return NotFound();
            }

            var projectgift = _context.Projectgifts.Where(x => x.Id == id).SingleOrDefault();
            if (projectgift == null)
            {
                return NotFound();
            }
            ViewData["gift"] = projectgift;
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditItem(decimal id, IFormFile ImageFile, string Name, string Description, decimal Price)
        {
            var projectgift = _context.Projectgifts.Where(x => x.Id == id).SingleOrDefault();
            if (projectgift == null)
            {
                return NotFound();
            }
            if (ImageFile != null)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + ImageFile.FileName;
                string path = Path.Combine(wwwRootPath + "/Images/GiftImages/", fileName);
                using (var filestream = new FileStream(path, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(filestream);
                }

                projectgift.Image = fileName;
            }
            if (!string.IsNullOrEmpty(Name))
            {
                projectgift.Name = Name;
            }

            if (!string.IsNullOrEmpty(Description))
            {
                projectgift.Description = Description;
            }

            if (Price.ToString() != null)
            {
                projectgift.Price = Price;
            }
            _context.Update(projectgift);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Gifts));

        }

        public IActionResult Notification()
        {
            decimal userId = Convert.ToDecimal(HttpContext.Session.GetString("userId"));
            int pendingPresentsCount = _context.Projectpresents
                .Where(p => p.Gifts.CategoryUsr.Userid == userId)
                .Count(p => p.Projectstatuses.Any(s => s.Notifecationstatus == "pending"));
            ViewData["Notification"] = pendingPresentsCount;
            var Home = _context.Projecthomes
                        .Include(p => p.Background)
                        .Include(p => p.Footer)
                        .Include(p => p.Header)
                        .Include(p => p.Contactus)
                        .Include(p => p.Aboutus)
                        .Where(p => p.Name == "Gift Maker").FirstOrDefault();
            ViewData["Home"] = Home;
            var presentStatuses = _context.Projectpresents
                               .Where(p => p.Gifts.CategoryUsr.Userid == userId)
                               .Select(p => new UserGiftPresentStatusClass
                               {
                                   PresentId = p.Id,
                                   Username = p.User.Username,
                                   Email = p.User.Email,
                                   GiftName = p.Gifts.Name,
                                   GiftPrice = p.Gifts.Price,
                                   RequestDate = p.Requestdate,
                                   ReciverAddress = p.Reciveraddress,
                                   ArrivedStatus = p.Projectstatuses.FirstOrDefault().Arrivedstatus,
                                   PaidStatus = p.Projectstatuses.FirstOrDefault().Paidstatus,
                                   RequestStatus = p.Projectstatuses.FirstOrDefault().Requeststatus,
                                   NotificationStatus = p.Projectstatuses.FirstOrDefault().Notifecationstatus
                               })
                               .OrderBy(p => p.NotificationStatus == "Read")
                               .ToList();
            if (TempData.ContainsKey("Success"))
            {
                ViewData["Success"] = TempData["Success"];
            }
            if (TempData.ContainsKey("Error"))
            {
                ViewData["Error"] = TempData["Error"];
            }
            return View(presentStatuses);

        }
        public IActionResult Read(decimal id)
        {
            var presentStatus = _context.Projectstatuses.FirstOrDefault(p => p.Presentid == id);
            if (presentStatus != null)
            {
                presentStatus.Notifecationstatus = "Read";
                _context.SaveChanges();
            }

            return RedirectToAction("Notification");
        }
        public async Task<IActionResult> Accept(decimal id)
        {
            var presentStatus = _context.Projectstatuses.FirstOrDefault(p => p.Presentid == id);
            if (presentStatus != null)
            {
                presentStatus.Requeststatus = "Accept";
                _context.SaveChanges();

                var present = _context.Projectpresents
                    .Include(p => p.User)
                    .FirstOrDefault(p => p.Id == id);

                if (present != null)
                {
                    var email = present.User.Email;
                    var message = new MailMessage();
                    message.From = new MailAddress("gifthavenjo@gmail.com", "GiftHaven");
                    message.To.Add(new MailAddress(email));
                    message.Subject = "Gift Status";
                    message.Body = $@"
                <html>
                    <body>
                        <p>Dear {present.User.Lname},</p>
                        <p>Your gift request has been accepted. Please proceed to make the payment:</p>
                        <a href=""{Url.Action("Payment", "GiftSender", new { id = present.Id }, Request.Scheme)}"">
                            <button style=""background-color: #4CAF50; color: white; padding: 10px 20px; border: none; border-radius: 4px;"">
                                Make Payment
                            </button>
                        </a>
                        <p>Thank you!</p>
                    </body>
                </html>";
                    message.IsBodyHtml = true;

                    await smtpClient.SendMailAsync(message);
                    TempData["Success"] = "Gifts Accepted Successfully";

                }
            }

            return RedirectToAction("Notification");
        }

        public async Task<IActionResult> Deny(decimal id)
        {
            var presentStatus = _context.Projectstatuses.FirstOrDefault(p => p.Presentid == id);
            if (presentStatus != null)
            {
                presentStatus.Requeststatus = "Deny";
                _context.SaveChanges();

                var present = _context.Projectpresents
                    .Include(p => p.User)
                    .FirstOrDefault(p => p.Id == id);

                if (present != null)
                {
                    var email = present.User.Email;
                    var message = new MailMessage();
                    message.From = new MailAddress("gifthavenjo@gmail.com", "GiftHaven");
                    message.To.Add(new MailAddress(email));
                    message.Subject = "Gift Status";
                    message.Body = $@"
                <html>
                    <body>
                        <p>Dear {present.User.Fname} {present.User.Lname},</p>
                        <p>Your gift request has been denied. We apologize for any inconvenience caused.</p>
                        <p>Thank you.</p>
                    </body>
                </html>";
                    message.IsBodyHtml = true;
                    await smtpClient.SendMailAsync(message);
                    TempData["Success"] = "Gifts Denied Successfully";
                    return RedirectToAction("Notification");

                }
            }

            return RedirectToAction("Notification");
        }
        public async Task<IActionResult> Arrive(decimal id)
        {
            var presentStatus = _context.Projectstatuses.FirstOrDefault(p => p.Presentid == id);
            if (presentStatus != null && presentStatus.Paidstatus == "Paid")
            {
                presentStatus.Arrivedstatus = "Arrived";
                _context.SaveChanges();
                var present = _context.Projectpresents
                            .Include(p => p.User)
                            .FirstOrDefault(p => p.Id == id);

                if (present != null)
                {
                    var email = present.User.Email;
                    var message = new MailMessage();
                    message.From = new MailAddress("gifthavenjo@gmail.com", "GiftHaven");
                    message.To.Add(new MailAddress(email));
                    message.Subject = "Gift Status";
                    message.Body = $@"
                <html>
                    <body>
                        <p>Dear {present.User.Fname} {present.User.Lname},</p>
                        <p>Your gift request has arrived at its destination.</p>
                        <p>Thank you for using our service. We hope the recipient enjoys the gift!</p>
                    </body>
                </html>";

                    message.IsBodyHtml = true;
                    await smtpClient.SendMailAsync(message);
                    TempData["Success"] = "Gifts Arrived Successfully";
                    return RedirectToAction("Notification");

                }

            }
            else if (presentStatus.Paidstatus != "Paid")
            {
                TempData["Error"] = "You Can't Send The Gift Before The user Paying for it ";
                return RedirectToAction("Notification");
            }

            return RedirectToAction("Notification");
        }
        public IActionResult Aboutus()
        {
            decimal userId = Convert.ToDecimal(HttpContext.Session.GetString("userId"));
            int pendingPresentsCount = _context.Projectpresents
                .Where(p => p.Gifts.CategoryUsr.Userid == userId)
                .Count(p => p.Projectstatuses.Any(s => s.Notifecationstatus == "pending"));
            ViewData["Notification"] = pendingPresentsCount;
            var Home = _context.Projecthomes
                        .Include(p => p.Background)
                        .Include(p => p.Footer)
                        .Include(p => p.Header)
                        .Include(p => p.Contactus)
                        .Include(p => p.Aboutus)
                        .Where(p => p.Name == "Gift Maker").FirstOrDefault();
            ViewData["Home"] = Home;
            return View();
        }
        public IActionResult Contactus()
        {
            decimal userId = Convert.ToDecimal(HttpContext.Session.GetString("userId"));
            int pendingPresentsCount = _context.Projectpresents
                .Where(p => p.Gifts.CategoryUsr.Userid == userId)
                .Count(p => p.Projectstatuses.Any(s => s.Notifecationstatus == "pending"));
            ViewData["Notification"] = pendingPresentsCount;

            var Home = _context.Projecthomes
                        .Include(p => p.Background)
                        .Include(p => p.Footer)
                        .Include(p => p.Header)
                        .Include(p => p.Contactus)
                        .Include(p => p.Aboutus)
                        .Where(p => p.Name == "Gift Maker").FirstOrDefault();
            ViewData["Home"] = Home;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contactus(string subject, string email, string message, string name)
        {
            decimal userId = Convert.ToDecimal(HttpContext.Session.GetString("userId"));
            int pendingPresentsCount = _context.Projectpresents
                .Where(p => p.Gifts.CategoryUsr.Userid == userId)
                .Count(p => p.Projectstatuses.Any(s => s.Notifecationstatus == "pending"));
            ViewData["Notification"] = pendingPresentsCount;

            var Home = _context.Projecthomes
                        .Include(p => p.Background)
                        .Include(p => p.Footer)
                        .Include(p => p.Header)
                        .Include(p => p.Contactus)
                        .Include(p => p.Aboutus)
                        .Where(p => p.Name == "Gift Maker").FirstOrDefault();
            ViewData["Home"] = Home;
            if (!string.IsNullOrEmpty(subject) && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(name))
            {
                var message1 = new MailMessage();
                message1.From = new MailAddress("gifthavenjo@gmail.com", "GiftHaven");
                message1.To.Add(new MailAddress("seewars73@gmail.com"));
                message1.Subject = "Contact Us" + subject;
                message1.Body = $"Name: {name}\nEmail: {email}\n\n{message}";

                await smtpClient.SendMailAsync(message1);

                ViewData["Success"] = "Thank You for Contacting Us!We appreciate your message and will get back to you as soon as possible.";
                return View();

            }
            else
            {
                ViewData["Error"] = "Please Enter All Of Your Information.";
                return View();
            }
        }
        public IActionResult Testimonial()
        {
            decimal userId = Convert.ToDecimal(HttpContext.Session.GetString("userId"));
            int pendingPresentsCount = _context.Projectpresents
                                        .Where(p => p.Gifts.CategoryUsr.Userid == userId)
                                        .Count(p => p.Projectstatuses.Any(s => s.Notifecationstatus == "pending"));
            ViewData["Notification"] = pendingPresentsCount;

            var Home = _context.Projecthomes
                        .Include(p => p.Background)
                        .Include(p => p.Footer)
                        .Include(p => p.Header)
                        .Include(p => p.Contactus)
                        .Include(p => p.Aboutus)
                        .Where(p => p.Name == "Gift Maker").FirstOrDefault();
            ViewData["Home"] = Home;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Testimonial([Bind("Description")] Projecttestimonial projecttestimonial)
        {
            decimal userId = Convert.ToDecimal(HttpContext.Session.GetString("userId"));
            int pendingPresentsCount = _context.Projectpresents
                                    .Where(p => p.Gifts.CategoryUsr.Userid == userId)
                                    .Count(p => p.Projectstatuses.Any(s => s.Notifecationstatus == "pending"));
            ViewData["Notification"] = pendingPresentsCount;

            var Home = _context.Projecthomes
            .Include(p => p.Background)
            .Include(p => p.Footer)
            .Include(p => p.Header)
            .Include(p => p.Contactus)
            .Include(p => p.Aboutus)
            .Where(p => p.Name == "Gift Maker").FirstOrDefault();
            ViewData["Home"] = Home;

            if (string.IsNullOrEmpty(projecttestimonial.Description))
            {
                ViewData["Error"] = "Please Enter Yor Feedback";
                return View();
            }
            projecttestimonial.UserId = Convert.ToDecimal(HttpContext.Session.GetString("userId"));
            projecttestimonial.Satatus = "pending";
            _context.Add(projecttestimonial);
            await _context.SaveChangesAsync();
            ViewData["Success"] = "Thank you for your feedback! Your testimonial has been submitted successfully.";
            return View();
        }

    }
}
