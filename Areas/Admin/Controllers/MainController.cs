using Microsoft.AspNetCore.Mvc;

namespace RezervationApp.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class MainController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
