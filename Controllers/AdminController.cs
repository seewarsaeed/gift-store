using GiftStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using NuGet.Protocol.Plugins;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using Syncfusion.UI.Xaml.Charts;
using System.Reflection.Metadata;
using System;
using System.IO;
using System.Linq;
using Syncfusion.Blazor.Charts;
using Syncfusion.Drawing;
using Rotativa.AspNetCore;
using System.Globalization;
using PdfSharpCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing.Pdf;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Png;

namespace GiftStore.Controllers
{
    public class AdminController : Controller
    {
        private readonly ModelContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _contextAccessor;

        public AdminController(ModelContext context, ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _contextAccessor = contextAccessor;
        }
        SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587)
        {
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential("gifthavenjo@gmail.com", "rjoahehjvocuiose"),
            EnableSsl = true
        };
        public IActionResult Index()
        {
            int userCount = _context.Projectusers.Count() - 1;
            int giftCount = _context.Projectgifts.Count();
            int categoryCount = _context.Projectcategories.Count();
            ViewBag.UserCount = userCount;
            ViewBag.GiftCount = giftCount;
            ViewBag.CategoryCount = categoryCount;
            ViewBag.username = HttpContext.Session.GetString("username");
            List<Projectcategoryuser> pendingGiftMakers = _context.Projectcategoryusers
             .Include(p => p.User)
             .Include(p => p.Category)
             .Where(p => p.User.Status == "Pending")
             .ToList();
            ViewData["pendingGiftMakers"] = pendingGiftMakers;

            var paidGiftIds = _context.Projectstatuses
                .Where(status => status.Paidstatus == "Paid")
                .Select(status => status.Presentid)
                .Distinct()
                .ToList();

            var giftData = _context.Projectpresents
                .Join(_context.Projectgifts,
                    present => present.Giftsid,
                    gift => gift.Id,
                    (present, gift) => new
                    {
                        GiftName = gift.Name,
                        Quantity = 1,
                        Price = gift.Price,
                        Presentid = present.Id
                    })
                .Where(data => paidGiftIds.Contains(data.Presentid))
                .GroupBy(data => new { data.GiftName, data.Price })
                .Select(group => new Tuple<string, int, decimal, decimal>(
                    group.Key.GiftName,
                    group.Sum(data => data.Quantity),
                    group.Key.Price ?? 0m,
                    group.Sum(data => data.Quantity) * (group.Key.Price ?? 0m) * 0.05m
                ))
                .ToList();
            ViewData["totalProfits"] = giftData.Sum(data => data.Item4);

            var pendingTestimonial = _context.Projecttestimonials
                                    .Include(p => p.User)
                                    .Where(p => p.Satatus == "pending")
                                    .ToList();
            ViewData["pendingTestimonial"] = pendingTestimonial;

            var projectcategoryusers = _context.Projectcategoryusers.Include(p => p.Category).Include(p => p.User).ToList();
            ViewData["categoryUser"] = projectcategoryusers;
            return View(giftData);
        }
        public async Task<IActionResult> RejectGiftMaker(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryUser = await _context.Projectcategoryusers
                .Include(cu => cu.User)
                .FirstOrDefaultAsync(cu => cu.Id == id);
            if (categoryUser == null)
            {
                return NotFound();
            }

            // Update the user's status to "Reject"
            categoryUser.User.Status = "Reject";

            await _context.SaveChangesAsync();

            var message = new MailMessage();
            message.From = new MailAddress("gifthavenjo@gmail.com", "GiftHaven");
            message.To.Add(new MailAddress(categoryUser.User.Email));
            message.Subject = "Account Status";
            message.Body = "\n\n\nDear " + categoryUser.User.Fname + categoryUser.User.Lname + "," + "\r\n\r\n\nWe regret to inform you that your application to join our team has been rejected. We carefully reviewed your information and qualifications, but unfortunately, we have decided not to proceed with your application at this time.\r\n\r\nWe appreciate your interest in joining our team and want to thank you for taking the time to apply. We encourage you to continue pursuing your goals and wish you success in your future endeavors.\r\n\r\nIf you have any further questions or would like feedback on your application, please feel free to reach out to us.\r\n\r\nThank you again for your interest.\r\n\r\nSincerely,\r\n\n The Gift Haven Team\r\n\r\n\n  ";
            await smtpClient.SendMailAsync(message);


            return RedirectToAction("Index");
        }
        public async Task<IActionResult> AcceptGiftMaker(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var categoryUser = await _context.Projectcategoryusers
                .Include(cu => cu.User)
                .FirstOrDefaultAsync(cu => cu.Id == id);
            if (categoryUser == null)
            {
                return NotFound();
            }

            // Update the user's status to "Available"
            categoryUser.User.Status = "Available";

            await _context.SaveChangesAsync();
            var message = new MailMessage();
            message.From = new MailAddress("gifthavenjo@gmail.com", "GiftHaven");
            message.To.Add(new MailAddress(categoryUser.User.Email));
            message.Subject = "Account Status";
            message.Body = "\n\n\nDear " + categoryUser.User.Fname + categoryUser.User.Lname + "," + "\r\n\r\n\nWe are pleased to inform you that your application to join our team has been accepted. Congratulations! We carefully reviewed your qualifications and believe that you will be a valuable addition to our organization.\r\n\r\nWe appreciate your interest in becoming a part of our team and are excited to have you onboard. Your dedication, skills, and experience make you an excellent fit for the position. We look forward to working with you and achieving great things together.\r\n\r\nPlease find attached further details regarding your onboarding process, including your start date, orientation schedule, and any additional information you may need. If you have any questions or require any clarifications, please don't hesitate to reach out to us.\r\n\r\nOnce again, congratulations on your acceptance! We are confident that you will contribute significantly to our team's success.\r\n\r\nWelcome aboard!\r\n\r\nThe Gift Haven Team\r\n\r\n\n ";
            await smtpClient.SendMailAsync(message);

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> acceptTestimonail(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var Testimonial = await _context.Projecttestimonials
                            .Include(cu => cu.User)
                            .FirstOrDefaultAsync(cu => cu.Id == id);
            if (Testimonial == null)
            {
                return NotFound();
            }
            Testimonial.Satatus = "Accept";

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");


        }
        public async Task<IActionResult> rejectTestimonail(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var Testimonial = await _context.Projecttestimonials
                            .Include(cu => cu.User)
                            .FirstOrDefaultAsync(cu => cu.Id == id);
            if (Testimonial == null)
            {
                return NotFound();
            }
            Testimonial.Satatus = "Reject";

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }
        public async Task<IActionResult> Gifts()
        {
            ViewBag.username = HttpContext.Session.GetString("username");

            var modelContext = _context.Projectgifts
                                .Include(p => p.CategoryUsr)
                                    .ThenInclude(cu => cu.Category)
                                .Include(p => p.CategoryUsr)
                                    .ThenInclude(cu => cu.User);
            return View(await modelContext.ToListAsync());

        }
        public IActionResult Profile()
        {
            
            ViewBag.username = HttpContext.Session.GetString("username");
            string ID = HttpContext.Session.GetString("userId");
            var user = _context.Projectusers.Where(p => p.Id.ToString() == ID).SingleOrDefault();
            if (user == null)
            {
                return NotFound();

            }
            ViewData["user"] = user;
            return View();
        }
        public IActionResult Edit()
        {
            ViewBag.username = HttpContext.Session.GetString("username");
            string ID = HttpContext.Session.GetString("userId");
            var user = _context.Projectusers.Where(p => p.Id.ToString() == ID).SingleOrDefault();
            if (user == null)
            {
                return NotFound();

            }
            ViewData["user"] = user;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IFormFile ImageFile, string username, string Fname, string Lname, string email, string pnumber, string Address, string oldPassword, string NewPassword, string reNewPassword)
        {
            ViewBag.username = HttpContext.Session.GetString("username");

            string ID = HttpContext.Session.GetString("userId");
            var user = _context.Projectusers.Where(p => p.Id.ToString() == ID).SingleOrDefault();
            ViewData["user"] = user;

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
            ViewData["user"] = user;
            ViewData["Error"] = "You shoul enter your current password correctly, try again please.";
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
        public IActionResult FinancialReports()
        {
            var ReportsList = _context.Projectpresents
                                         .Where(p => p.Projectstatuses.FirstOrDefault().Paidstatus == "Paid")
                                         .Select(p => new Reports
                                         {
                                             Username = p.User.Username,
                                             ReciverAddress = p.Reciveraddress,
                                             GiftName = p.Gifts.Name,
                                             RequestDate = p.Requestdate,
                                             PaidStatus = p.Projectstatuses.FirstOrDefault().Paidstatus,
                                             Price = p.Gifts.Price,
                                             Profits = p.Gifts.Price != null ? p.Gifts.Price.GetValueOrDefault() * 0.05m : null
                                         })
                                         .ToList();
            return View(ReportsList);

        }
        [HttpPost]
        public IActionResult FinancialReports(DateTime? startDate, DateTime? endDate)
        {
            if (startDate == null && endDate == null)
            {
                var ReportsList = _context.Projectpresents
                    .Where(p => p.Projectstatuses.FirstOrDefault().Paidstatus == "Paid")
                    .Select(p => new Reports
                    {
                        Username = p.User.Username,
                        ReciverAddress = p.Reciveraddress,
                        GiftName = p.Gifts.Name,
                        RequestDate = p.Requestdate,
                        PaidStatus = p.Projectstatuses.FirstOrDefault().Paidstatus,
                        Price = p.Gifts.Price,
                        Profits = p.Gifts.Price != null ? p.Gifts.Price.GetValueOrDefault() * 0.05m : null
                    })
                    .ToList();

                return View(ReportsList);
            }
            else if (startDate == null && endDate != null)
            {
                // Logic for filtering data based on endDate
                var ReportsList = _context.Projectpresents
                    .Where(p => p.Projectstatuses.FirstOrDefault().Paidstatus == "Paid" && p.Requestdate <= endDate)
                    .Select(p => new Reports
                    {
                        Username = p.User.Username,
                        ReciverAddress = p.Reciveraddress,
                        GiftName = p.Gifts.Name,
                        RequestDate = p.Requestdate,
                        PaidStatus = p.Projectstatuses.FirstOrDefault().Paidstatus,
                        Price = p.Gifts.Price,
                        Profits = p.Gifts.Price != null ? p.Gifts.Price.GetValueOrDefault() * 0.05m : null
                    })
                    .ToList();

                return View(ReportsList);
            }
            else if (startDate != null && endDate == null)
            {
                // Logic for filtering data based on startDate
                var ReportsList = _context.Projectpresents
                    .Where(p => p.Projectstatuses.FirstOrDefault().Paidstatus == "Paid" && p.Requestdate >= startDate)
                    .Select(p => new Reports
                    {
                        Username = p.User.Username,
                        ReciverAddress = p.Reciveraddress,
                        GiftName = p.Gifts.Name,
                        RequestDate = p.Requestdate,
                        PaidStatus = p.Projectstatuses.FirstOrDefault().Paidstatus,
                        Price = p.Gifts.Price,
                        Profits = p.Gifts.Price != null ? p.Gifts.Price.GetValueOrDefault() * 0.05m : null
                    })
                    .ToList();

                return View(ReportsList);
            }
            else
            {
                // Logic for filtering data based on both startDate and endDate
                var ReportsList = _context.Projectpresents
                    .Where(p => p.Projectstatuses.FirstOrDefault().Paidstatus == "Paid" && p.Requestdate >= startDate && p.Requestdate <= endDate)
                    .Select(p => new Reports
                    {
                        Username = p.User.Username,
                        ReciverAddress = p.Reciveraddress,
                        GiftName = p.Gifts.Name,
                        RequestDate = p.Requestdate,
                        PaidStatus = p.Projectstatuses.FirstOrDefault().Paidstatus,
                        Price = p.Gifts.Price,
                        Profits = p.Gifts.Price != null ? p.Gifts.Price.GetValueOrDefault() * 0.05m : null
                    })
                    .ToList();

                return View(ReportsList);
            }
        }
        [HttpGet("/admin/monthly-profits")]
        public IActionResult GetMonthlyProfits()
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            var monthlyProfits = _context.Projectpresents
                .Where(p => p.Projectstatuses.FirstOrDefault().Paidstatus == "Paid" && p.Requestdate.HasValue && p.Gifts.Price.HasValue)
                .Where(p => p.Requestdate.Value.Year == currentYear && p.Requestdate.Value.Month <= currentMonth)
                .GroupBy(p => p.Requestdate.Value.Month)
                .OrderBy(p => p.Key)
                .Select(g => new { Month = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(g.Key), Profit = g.Sum(p => p.Gifts.Price.Value * 0.05m) })
                .ToList();
            return Json(new { labels = monthlyProfits.Select(p => p.Month), values = monthlyProfits.Select(p => p.Profit) });
        }
        public class DownloadPDFRequest
        {
            public string canvas { get; set; }
        }
        [HttpPost("/admin/download-pdf")]
        public IActionResult DownloadPDF([FromBody] DownloadPDFRequest data)
        {
            var imageData = data.canvas;

            // Decode the Base64 string into bytes
            var bytes = Convert.FromBase64String(imageData);

            // Generate a unique file name for the image
            var fileName = $"{Guid.NewGuid()}.png";

            // Save the image file on the server
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            System.IO.File.WriteAllBytes(filePath, bytes);

            // Create a new PDF document
            var document = new PdfDocument();
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);

            // Draw the heading on the PDF page
            var font = new XFont("Arial", 16, XFontStyle.Bold);
            var heading = "Monthly Profits Report";
            var headingRect = new XRect(10, 10, page.Width - 20, 50);
            gfx.DrawString(heading, font, XBrushes.Black, headingRect, XStringFormats.Center);

            // Calculate the target width and height for the image based on 80% of the PDF page size
            var targetWidth = page.Width * 0.8;
            var targetHeight = page.Height * 0.8;

            // Load the image using SixLabors.ImageSharp
            using (var image = Image.Load(filePath))
            {
                // Adjust image size to fit the PDF page
                image.Mutate(x => x.Resize((int)targetWidth, (int)targetHeight));

                // Convert SixLabors.ImageSharp image to bytes
                using (var outputStream = new MemoryStream())
                {
                    image.Save(outputStream, new PngEncoder());

                    // Convert the image bytes to XImage using a Func<Stream> delegate
                    var xImage = XImage.FromStream(() => new MemoryStream(outputStream.ToArray()));
                    // Calculate the position to center the image on the PDF page
                    var xPos = (page.Width - targetWidth) / 2;
                    var yPos = (page.Height - targetHeight) / 2;

                    // Draw the image on the PDF page
                    gfx.DrawImage(xImage, new XRect(xPos, yPos, targetWidth, targetHeight));
                }
            }

            // Delete the temporary image file
            System.IO.File.Delete(filePath);

            using (var stream = new MemoryStream())
            {
                document.Save(stream);
                bytes = stream.ToArray();

                // Return the PDF file as a response
                return File(bytes, "application/pdf", "MonthlyChart.pdf");
            }
        }
        [HttpGet("/admin/annual-profits")]
        public IActionResult GetAnnualProfits()
        {
            var currentYear = DateTime.Now.Year;
            var pastYears = new List<int> { currentYear, currentYear - 1, currentYear - 2 };

            var annualProfits = _context.Projectpresents
                .Where(p => p.Projectstatuses.FirstOrDefault().Paidstatus == "Paid" && p.Requestdate.HasValue && p.Gifts.Price.HasValue)
                .Where(p => pastYears.Contains(p.Requestdate.Value.Year))
                .GroupBy(p => p.Requestdate.Value.Year)
                .OrderBy(p => p.Key)
                .Select(g => new { Year = g.Key, Profit = g.Sum(p => p.Gifts.Price.Value * 0.05m) })
                .ToList();
            return Json(new { labels = annualProfits.Select(p => p.Year), values = annualProfits.Select(p => p.Profit) });
        }
        [HttpPost("/admin/downloadAnnual-pdf")]
        public IActionResult DownloadAnuualPDF([FromBody] DownloadPDFRequest data)
        {
            var imageData = data.canvas;

            // Decode the Base64 string into bytes
            var bytes = Convert.FromBase64String(imageData);

            // Generate a unique file name for the image
            var fileName = $"{Guid.NewGuid()}.png";

            // Save the image file on the server
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            System.IO.File.WriteAllBytes(filePath, bytes);

            // Create a new PDF document
            var document = new PdfDocument();
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);

            // Draw the heading on the PDF page
            var font = new XFont("Arial", 16, XFontStyle.Bold);
            var heading = "Anuual Profits Report";
            var headingRect = new XRect(10, 10, page.Width - 20, 50);
            gfx.DrawString(heading, font, XBrushes.Black, headingRect, XStringFormats.Center);

            // Calculate the target width and height for the image based on 80% of the PDF page size
            var targetWidth = page.Width * 0.8;
            var targetHeight = page.Height * 0.8;

            // Load the image using SixLabors.ImageSharp
            using (var image = Image.Load(filePath))
            {
                // Adjust image size to fit the PDF page
                image.Mutate(x => x.Resize((int)targetWidth, (int)targetHeight));

                // Convert SixLabors.ImageSharp image to bytes
                using (var outputStream = new MemoryStream())
                {
                    image.Save(outputStream, new PngEncoder());

                    // Convert the image bytes to XImage using a Func<Stream> delegate
                    var xImage = XImage.FromStream(() => new MemoryStream(outputStream.ToArray()));

                    // Calculate the position to center the image on the PDF page
                    var xPos = (page.Width - targetWidth) / 2;
                    var yPos = (page.Height - targetHeight) / 2;

                    // Draw the image on the PDF page
                    gfx.DrawImage(xImage, new XRect(xPos, yPos, targetWidth, targetHeight));
                }
            }

            // Delete the temporary image file
            System.IO.File.Delete(filePath);

            using (var stream = new MemoryStream())
            {
                document.Save(stream);
                bytes = stream.ToArray();

                // Return the PDF file as a response
                return File(bytes, "application/pdf", "AnuualChart.pdf");
            }
        }
        public IActionResult ManageWebsitePages()
        {
            var aboutus = _context.Projectaboutus.ToList();
            ViewData["aboutus"] = aboutus;
            var contactus = _context.Projectcontactus.ToList();
            ViewData["contactus"] = contactus;
            var testimonial = _context.Projecttestimonials.Include(p=>p.User).ToList();
            ViewData["testimonial"] = testimonial;
            var header = _context.Projectheaders.ToList();
            ViewData["header"] = header;
            var footer = _context.Projectfooters.ToList();
            ViewData["footer"] = footer;
            var background = _context.Projectbackgrounds.ToList();
            ViewData["background"] = background;
            var Home = _context.Projecthomes
                        .Include(p => p.Aboutus)
                        .Include(p => p.Contactus)
                        .Include(p => p.Testimonial)
                        .Include(p => p.Header)
                        .Include(p => p.Footer)
                        .Include(p => p.Background).ToList();
            return View(Home);
        }

    }


}
