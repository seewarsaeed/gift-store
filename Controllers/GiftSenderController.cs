using GiftStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace GiftStore.Controllers
{
    public class GiftSenderController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<HomeController> _logger;

        public GiftSenderController(ModelContext context, IWebHostEnvironment webHostEnvironment, ILogger<HomeController> logger)
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
        private PdfDocument GenerateInvoicePdf(Projectpresent present)
        {
            var document = new PdfDocument();
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);
            var font = new XFont("Arial", 12);

            var thankYouMessage = "Thank you for your purchase!";
            var fontSize = 16;
            var fontBold = new XFont("Arial", fontSize, XFontStyle.Bold);
            var messageSize = gfx.MeasureString(thankYouMessage, fontBold);
            var x = (page.Width - messageSize.Width) / 2;
            var y = 150;
            gfx.DrawString(thankYouMessage, fontBold, XBrushes.Black, new XPoint(x, y));

            var invoiceTitleFont = new XFont("Arial", 16, XFontStyle.Bold);
            var invoiceContentFont = new XFont("Arial", 12);

            gfx.DrawString("Invoice", invoiceTitleFont, XBrushes.Black, new XPoint(50, 50));
            gfx.DrawString($"Gift: {present.Gifts.Name}", invoiceContentFont, XBrushes.Black, new XPoint(50, 80));
            gfx.DrawString($"Price: {present.Gifts.Price}", invoiceContentFont, XBrushes.Black, new XPoint(50, 100));

            return document;
        }

        public IActionResult Index()
        {
            var Home = _context.Projecthomes
                .Include(p => p.Background)
                .Include(p => p.Footer)
                .Include(p => p.Header)
                .Include(p => p.Contactus)
                .Include(p => p.Aboutus)
                .Where(p => p.Name == "Gift Sender").FirstOrDefault();
            ViewData["Home"] = Home;
            var Testimonial = _context.Projecttestimonials.Include(p => p.User).Where(p => p.Satatus == "Accept").ToList();
            ViewData["Testimonial"] = Testimonial;
            var Categories = _context.Projectcategories.ToList();
            ViewData["Categories"] = Categories;

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
             .Where(p => p.Name == "Gift Sender").FirstOrDefault();
            ViewData["Home"] = Home;

            string ID = HttpContext.Session.GetString("userId");
            var user = _context.Projectusers.Where(p => p.Id.ToString() == ID).SingleOrDefault();
            ViewData["user"] = user;
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
                         .Where(p => p.Name == "Gift Sender").FirstOrDefault();
                            ViewData["Home"] = Home;
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
        public IActionResult Product(decimal? id, string? gift)
        {
            var Home = _context.Projecthomes
             .Include(p => p.Background)
             .Include(p => p.Footer)
             .Include(p => p.Header)
             .Include(p => p.Contactus)
             .Include(p => p.Aboutus)
             .Where(p => p.Name == "Gift Sender").FirstOrDefault();
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
        public async Task<IActionResult> SingleProduct(decimal? id)
        {
            var Home = _context.Projecthomes
             .Include(p => p.Background)
             .Include(p => p.Footer)
             .Include(p => p.Header)
             .Include(p => p.Contactus)
             .Include(p => p.Aboutus)
             .Where(p => p.Name == "Gift Sender").FirstOrDefault();
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
            if (TempData.ContainsKey("Error"))
            {
                ViewData["Error"] = TempData["Error"];
            }
            return View(gift);
        }
        public async Task<IActionResult> AddCart(decimal? id)
        {
            var Home = _context.Projecthomes
             .Include(p => p.Background)
             .Include(p => p.Footer)
             .Include(p => p.Header)
             .Include(p => p.Contactus)
             .Include(p => p.Aboutus)
             .Where(p => p.Name == "Gift Sender").FirstOrDefault();
            ViewData["Home"] = Home;

            if (id == null)
            {
                return NotFound();
            }
            string userId = HttpContext.Session.GetString("userId");
            var visa = _context.Projectpayments.FirstOrDefault(p => p.Userid.ToString() == userId);
            if (visa == null)
            {
                TempData["Error"] = "You Should Add Your Visa Before";
                return RedirectToAction("SingleProduct", new { id = id });
            }

            var gift = await _context.Projectgifts.FirstOrDefaultAsync(m => m.Id == id);

            if (gift == null)
            {
                return NotFound();
            }

            return View("Cart", gift);
        }
        public IActionResult Cart()
        {
            var Home = _context.Projecthomes
             .Include(p => p.Background)
             .Include(p => p.Footer)
             .Include(p => p.Header)
             .Include(p => p.Contactus)
             .Include(p => p.Aboutus)
             .Where(p => p.Name == "Gift Sender").FirstOrDefault();
            ViewData["Home"] = Home;

            return View();
        }
        public async Task<IActionResult> Checkout(decimal? id)
        {
            var Home = _context.Projecthomes
             .Include(p => p.Background)
             .Include(p => p.Footer)
             .Include(p => p.Header)
             .Include(p => p.Contactus)
             .Include(p => p.Aboutus)
             .Where(p => p.Name == "Gift Sender").FirstOrDefault();
            ViewData["Home"] = Home;

            if (id == null)
            {
                return NotFound();
            }

            var gift = await _context.Projectgifts.FirstOrDefaultAsync(m => m.Id == id);

            if (gift == null)
            {
                return NotFound();
            }
            if (TempData.ContainsKey("Error"))
            {
                ViewData["Error"] = TempData["Error"];
            }
            TempData["Success"] = "Your Gift Request Successfully Done";
            return View(gift);
        }
        public IActionResult OrderIndex()
        {
            var Home = _context.Projecthomes
             .Include(p => p.Background)
             .Include(p => p.Footer)
             .Include(p => p.Header)
             .Include(p => p.Contactus)
             .Include(p => p.Aboutus)
             .Where(p => p.Name == "Gift Sender").FirstOrDefault();
            ViewData["Home"] = Home;

            string ID = HttpContext.Session.GetString("userId");

            if (ID == null)
            {
                return NotFound();
            }

            var userPresentStatusList = _context.Projectpresents
                .Where(p => p.Userid.ToString() == ID)
                .Select(p => new UserPresentStatusClass
                {
                    UserId = p.Userid.Value,
                    PresentId = p.Id,
                    ReciverAddress = p.Reciveraddress,
                    GiftName = p.Gifts.Name,
                    GiftId = p.Giftsid,
                    RequestDate = p.Requestdate,
                    ArrivedStatus = p.Projectstatuses.FirstOrDefault().Arrivedstatus,
                    PaidStatus = p.Projectstatuses.FirstOrDefault().Paidstatus,
                    RequestStatus = p.Projectstatuses.FirstOrDefault().Requeststatus,
                    NotificationStatus = p.Projectstatuses.FirstOrDefault().Notifecationstatus
                })
                .ToList();
            if (TempData.ContainsKey("Success"))
            {
                ViewData["Success"] = TempData["Success"];
            }
            return View(userPresentStatusList);
        }
        public async Task<IActionResult> Order(decimal? id, string address)
        {
            var Home = _context.Projecthomes
             .Include(p => p.Background)
             .Include(p => p.Footer)
             .Include(p => p.Header)
             .Include(p => p.Contactus)
             .Include(p => p.Aboutus)
             .Where(p => p.Name == "Gift Sender").FirstOrDefault();
            ViewData["Home"] = Home;
            if (id == null)
            {
                return NotFound();
            }
            if (address == null)
            {
                TempData["Error"] = "You Should Enter The Receiver Address Please";
                return RedirectToAction("Checkout", new { id = id });
            }

            Projectpresent projectpresent = new Projectpresent();
            projectpresent.Giftsid = id;
            projectpresent.Reciveraddress = address;
            projectpresent.Userid = Convert.ToDecimal(HttpContext.Session.GetString("userId"));
            projectpresent.Requestdate = DateTime.Now;
            _context.Add(projectpresent);
            await _context.SaveChangesAsync();

            Projectstatus projectstatus = new Projectstatus();
            projectstatus.Presentid = projectpresent.Id;
            projectstatus.Arrivedstatus = "pending";
            projectstatus.Notifecationstatus = "pending";
            projectstatus.Paidstatus = "pending";
            projectstatus.Requeststatus = "pending";
            _context.Add(projectstatus);
            await _context.SaveChangesAsync();

            //var gift = await _context.Projectgifts.FirstOrDefaultAsync(m => m.Id == id);

            return RedirectToAction("OrderIndex");
        }
        public IActionResult Payment(decimal? id)
        {
            var Home = _context.Projecthomes
             .Include(p => p.Background)
             .Include(p => p.Footer)
             .Include(p => p.Header)
             .Include(p => p.Contactus)
             .Include(p => p.Aboutus)
             .Where(p => p.Name == "Gift Sender").FirstOrDefault();
            ViewData["Home"] = Home;

            string ID = HttpContext.Session.GetString("userId");
            if (ID == null || id == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            var projectpresent = _context.Projectpresents
                .Include(p => p.Gifts)
                .FirstOrDefault(p => p.Id == id);
            if (TempData.ContainsKey("Error"))
            {
                ViewData["Error"] = TempData["Error"];
            }
            return View(projectpresent);
        }
        public async Task<IActionResult> Cancel(decimal? id)
        {
            var present = _context.Projectpresents.FirstOrDefault(p => p.Id == id);
            var presentStatus = _context.Projectstatuses.FirstOrDefault(ps => ps.Presentid == id);

            presentStatus.Paidstatus = "Canceled";
            var email = present.User.Email;
            var message = new MailMessage();
            message.From = new MailAddress("gifthavenjo@gmail.com", "GiftHaven");
            message.To.Add(new MailAddress(email));
            message.Subject = "Gift Status";
            message.Body = $@"
                <html>
                    <body>
                        <p>Dear {present.User.Fname} {present.User.Lname} ,</p>
                            <p>We regret to inform you that your gift request has been cancelled.</p>
                            <p>We apologize for any inconvenience caused. If you have any questions or need further assistance, please don't hesitate to contact our customer support.</p>
                            <p>Thank you for using our service. We appreciate your understanding and hope to serve you again in the future.</p>
                            <p>Best regards,</p>
                            <p>Your GiftHaven Team</p>
                    </body>
                </html>";

            message.IsBodyHtml = true;
            await smtpClient.SendMailAsync(message);
            _context.SaveChanges();
            TempData["Success"] = "Your Order Cancelled Successfully.";
            return RedirectToAction("OrderIndex");
        }
        public async Task<IActionResult> Pay(decimal? id)
        {
            string userId = HttpContext.Session.GetString("userId");
            var visa = _context.Projectpayments.FirstOrDefault(p => p.Userid.ToString() == userId);
            var present = _context.Projectpresents
                            .Include(p => p.Gifts)
                            .Include(p => p.User)
                            .FirstOrDefault(p => p.Id == id);
            var presentStatus = _context.Projectstatuses.FirstOrDefault(ps => ps.Presentid == id);
            if (visa.Availablebalance < present.Gifts.Price)
            {
                TempData["Error"] = "This Process Can't be Done, There Is No Enough Balance in Your Visa. ";
                return RedirectToAction("OrderIndex");
            }
            if (visa.Expirationdate < DateTime.Now)
            {
                TempData["Error"] = "This Process Can't be Done, Your Visa Has Expired. ";
                return RedirectToAction("OrderIndex");
            }
            if (visa.Availablebalance >= present.Gifts.Price && visa.Expirationdate >= DateTime.Now)
            {
                // Deduct the payment amount from the user's available balance
                visa.Availablebalance -= present.Gifts.Price;

                // Update the present status as "paid"
                presentStatus.Paidstatus = "Paid";

                _context.SaveChanges();

                var email = present.User?.Email;

                // Generate the PDF invoice
                var invoicePdf = GenerateInvoicePdf(present);

                var message = new MailMessage();
                message.From = new MailAddress("gifthavenjo@gmail.com", "GiftHaven");
                message.To.Add(new MailAddress(email));
                message.Subject = "Payment Confirmation and Invoice";
                message.Body = $@"
            <html>
                <body>
                    <p>Dear {present.User.Fname} {present.User.Lname},</p>
                    <p>Your payment has been confirmed. Thank you for your purchase.</p>
                    <p>Please find attached your invoice.</p>
                    <p>Thank you!</p>
                </body>
            </html>";
                message.IsBodyHtml = true;

                // Attach the PDF invoice to the email
                var memoryStream = new MemoryStream();
                invoicePdf.Save(memoryStream, false);
                memoryStream.Position = 0;
                message.Attachments.Add(new Attachment(memoryStream, "invoice.pdf", "application/pdf"));

                await smtpClient.SendMailAsync(message);
            }
            TempData["Success"] = "Payment Process Done, Check Your Email If You Want to See The Invoice.";
            return RedirectToAction("OrderIndex");
        }
        public IActionResult Visa()
        {
            var Home = _context.Projecthomes
             .Include(p => p.Background)
             .Include(p => p.Footer)
             .Include(p => p.Header)
             .Include(p => p.Contactus)
             .Include(p => p.Aboutus)
             .Where(p => p.Name == "Gift Sender").FirstOrDefault();
            ViewData["Home"] = Home;

            string ID = HttpContext.Session.GetString("userId");
            var visaDetails = _context.Projectpayments.Where(p => p.Userid.ToString() == ID).Include(p => p.User).SingleOrDefault();
            ViewData["visaDetails"] = visaDetails;
            if (TempData.ContainsKey("Error"))
            {
                ViewData["Error"] = TempData["Error"];
            }
            if (TempData.ContainsKey("Check"))
            {
                ViewData["Check"] = TempData["Check"];
            }
            else
            {
                ViewData["Check"] = "False";
            }
            if (TempData.ContainsKey("Success"))
            {
                ViewData["Success"] = TempData["Success"];
            }

            return View();
        }
        [HttpPost]
        public IActionResult CheckPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                TempData["Error"] = "Please Enter Your Password";
                return RedirectToAction("Visa");
            }

            string ID = HttpContext.Session.GetString("userId");
            var visaDetails = _context.Projectpayments
                .Where(p => p.Userid.ToString() == ID)
                .Include(p => p.User)
                .SingleOrDefault();
            var user = _context.Projectusers
                     .Where(p => p.Id.ToString() == ID)
                     .SingleOrDefault();


            if (password != user.Password)
            {
                TempData["Error"] = "Wrong Password, Try Again Please.";
                return RedirectToAction("Visa");
            }

            TempData["Check"] = "True";
            return RedirectToAction("Visa");
        }
        [HttpPost]
        public async Task<IActionResult> EditVisa(string cnumber, decimal balance, string cvv)
        {
            string ID = HttpContext.Session.GetString("userId");
            var visaDetails = _context.Projectpayments.Where(p => p.Userid.ToString() == ID).Include(p => p.User).SingleOrDefault();
            if (visaDetails == null)
            {
                visaDetails = new Projectpayment();
                visaDetails.Userid = Convert.ToDecimal(ID);
            }
            if (!string.IsNullOrEmpty(cnumber))
            {
                if (cnumber.Length > 16)
                {
                    TempData["Error"] = "Card Number Must be 16 Characters.";
                    return RedirectToAction("Visa");
                }
                visaDetails.Cardnumber = cnumber;
            }
            if (balance != 0)
            {
                visaDetails.Availablebalance = balance;
            }
            if (!string.IsNullOrEmpty(cvv))
            {
                if (cvv.Length > 4)
                {
                    TempData["Error"] = "CVV Must be 4 Characters";
                    return RedirectToAction("Visa");
                }
                visaDetails.Cvvcvc = cvv;
            }
            if (visaDetails.Expirationdate == null)
            {
                visaDetails.Expirationdate = DateTime.Now.AddYears(4);
            }
            _context.Update(visaDetails);
            await _context.SaveChangesAsync();
            TempData["Check"] = "True";
            TempData["Success"] = "Your Visa Edited Successfully";
            return RedirectToAction("Visa");
        }
        public IActionResult Aboutus()
        {
            var Home = _context.Projecthomes
                        .Include(p => p.Background)
                        .Include(p => p.Footer)
                        .Include(p => p.Header)
                        .Include(p => p.Contactus)
                        .Include(p => p.Aboutus)
                        .Where(p => p.Name == "Gift Sender").FirstOrDefault();
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
                        .Where(p => p.Name == "Gift Sender").FirstOrDefault();
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
            .Where(p => p.Name == "Gift Sender").FirstOrDefault();
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
            var Home = _context.Projecthomes
                        .Include(p => p.Background)
                        .Include(p => p.Footer)
                        .Include(p => p.Header)
                        .Include(p => p.Contactus)
                        .Include(p => p.Aboutus)
                        .Where(p => p.Name == "Gift Sender").FirstOrDefault();
            ViewData["Home"] = Home;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Testimonial([Bind("Description")] Projecttestimonial projecttestimonial)
        {
            var Home = _context.Projecthomes
            .Include(p => p.Background)
            .Include(p => p.Footer)
            .Include(p => p.Header)
            .Include(p => p.Contactus)
            .Include(p => p.Aboutus)
            .Where(p => p.Name == "Gift Sender").FirstOrDefault();
            ViewData["Home"] = Home;

            if (string.IsNullOrEmpty(projecttestimonial.Description))
            {
                ViewData["Error"] = "Please Enter Yor Feedback";
                return View();
            }
            projecttestimonial.UserId =Convert.ToDecimal(HttpContext.Session.GetString("userId"));
            projecttestimonial.Satatus = "pending";
            _context.Add(projecttestimonial);
            await _context.SaveChangesAsync();
            ViewData["Success"] = "Thank you for your feedback! Your testimonial has been submitted successfully.";
            return View();
        }
    }
}
