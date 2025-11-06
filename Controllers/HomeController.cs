using Microsoft.AspNetCore.Mvc;
using RezervationApp.Data;
using RezervationApp.Models;
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
