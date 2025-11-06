using Microsoft.AspNetCore.Mvc;
using RezervationApp.Data;

namespace RezervationApp.ViewComponents
{
	public class Services : ViewComponent
	{
		private readonly DatabaseContext _context;
		public Services(DatabaseContext context)
		{
			_context = context;
		}

		public IViewComponentResult Invoke()
		{
			return View(_context.Services.Where(c => c.IsActive && c.IsTopMenu));
		}
	}
}
