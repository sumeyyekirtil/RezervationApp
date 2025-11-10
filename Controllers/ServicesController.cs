using Microsoft.AspNetCore.Mvc;
using RezervationApp.Data;

namespace RezervationApp.Controllers
{
	public class ServicesController : Controller
	{
		private readonly DatabaseContext _context;

		public ServicesController(DatabaseContext context)
		{
			_context = context;
		}

		public IActionResult Index(string q = "")
		{
			return View(_context.Services.Where(a => a.IsActive && a.Name.Contains(q)).ToList());
		}

		public IActionResult Details(int? id)
		{

			return View(_context.Services.Find(id));
		}
	}
}
