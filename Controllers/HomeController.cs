using GiftStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;

namespace GiftStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ModelContext _context;
        SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587)
        {
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential("gifthavenjo@gmail.com", "rjoahehjvocuiose"),
            EnableSsl = true
        };

        public HomeController(ILogger<HomeController> logger, ModelContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var Home = _context.Projecthomes
                    .Include(p => p.Background)
                    .Include(p => p.Footer)
                    .Include(p => p.Header)
                    .Include(p => p.Contactus)
                    .Include(p => p.Aboutus)
                    .Where(p => p.Name == "Guest").FirstOrDefault();
            ViewData["Home"] = Home;
            var Testimonial = _context.Projecttestimonials.Include(p => p.User).Where(p => p.Satatus == "Accept").ToList();
            ViewData["Testimonial"] = Testimonial;
            var Categories = _context.Projectcategories.ToList();
            ViewData["Categories"] = Categories;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Aboutus()
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
        public IActionResult Contactus()
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contactus(string subject, string email, string message, string name)
        {
            var Home = _context.Projecthomes
            .Include(p => p.Background)
            .Include(p => p.Footer)
            .Include(p => p.Header)
            .Include(p => p.Contactus)
            .Include(p => p.Aboutus)
            .Where(p => p.Name == "Guest").FirstOrDefault();
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
        public IActionResult Product(decimal? id, string? gift)
        {
            var Home = _context.Projecthomes
             .Include(p => p.Background)
             .Include(p => p.Footer)
             .Include(p => p.Header)
             .Include(p => p.Contactus)
             .Include(p => p.Aboutus)
             .Where(p => p.Name == "Guest").FirstOrDefault();
            ViewData["Home"] = Home;

            var gifts = _context.Projectgifts.Include(p => p.CategoryUsr).ToList();
            if (id != null && gift == null)
            {
                gifts = _context.Projectgifts.Include(p => p.CategoryUsr).Where(g => g.CategoryUsr.Categoryid == id).ToList();
            }
            else if (gift != null)
            {
                gifts = _context.Projectgifts.Where(g => EF.Functions.Like(g.Name.ToLower(), $"%{gift.ToLower()}%")).ToList();

            }
            var category = _context.Projectcategories.ToList();
            ViewData["category"] = category;
            return View("Product", gifts);
        }
    }
}