using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RezervationApp.Data;
using RezervationApp.Entities;

namespace RezervationApp.Controllers
{
	[Authorize]
	public class RezervationsController : Controller
	{
		private readonly DatabaseContext _context;
		public RezervationsController(DatabaseContext context)
		{
			_context = context;
		}
		
		public IActionResult Index()
		{
			ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Email");
			return View();
		}
		
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Index(Rezervation rezervation)
		{
			if (ModelState.IsValid)
			{
				await _context.AddAsync(rezervation);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Email", rezervation.EmployeeId);
			return View(rezervation);
		}
	}
}
