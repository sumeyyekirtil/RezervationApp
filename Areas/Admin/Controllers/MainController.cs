using Microsoft.AspNetCore.Mvc;

namespace RezervationApp.Areas.Admin.Controllers
{
	public class MainController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
