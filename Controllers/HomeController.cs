using Microsoft.AspNetCore.Mvc;
using RezervationApp.Data;
using RezervationApp.Models;
using RezervationApp.Tools;
using System.Diagnostics;

namespace RezervationApp.Controllers
{
    public class HomeController : Controller
    {
		private readonly DatabaseContext _context;

		public HomeController(DatabaseContext context)
		{
			_context = context;
		}

		public IActionResult Index()
        {
            var model = new HomePageViewModel
            {
                Sliders = _context.Sliders.ToList(), //null referance exceptions error is solved!
                Services = _context.Services.Where(x => x.IsHome && x.IsActive).ToList()
            };
			return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        
        public IActionResult ContactUs()
        {
            return View();
        }

		[HttpPost]
		public IActionResult ContactUs(string nameSurname, string email, string message)
		{
			string mesaj = $"Ad Soyad:  {nameSurname} <hr> E mail:  {email} <hr> Mesaj:  {message}";
			try
			{
				MailHelper.SendMail("mail@gmail.com", "Siteden email geldi", message);
				TempData["Message"] = @"<div class=""alert alert-success alert-dismissible fade show"" role=""alert"">
                     <strong>Teþekkürler! Mesajýnýz Ýletildi!</strong>
                     <button type=""button"" class=""btn-close"" data-bs-dismiss=""alert"" aria-label=""Close""></button>
                     </div>";
			}
			catch (Exception)
			{
				TempData["Message"] = @"<div class=""alert alert-danger alert-dismissible fade show"" role=""alert"">
                     <strong>Hata Oluþtu! Mesaj gönderilemedi!</strong>
                     <button type=""button"" class=""btn-close"" data-bs-dismiss=""alert"" aria-label=""Close""></button>
                     </div>";
			}
			return RedirectToAction("ContactUs");
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
