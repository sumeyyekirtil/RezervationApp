using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RezervationApp.Data;
using RezervationApp.Dtos;
using RezervationApp.Entities;
using System.Security.Claims;

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
			//id gibi herhangi bir değere hata bazında ulaşmak için önce tanımlanan yeri start ile başlatmalı, ikinci adına breakpoint koyulmalı
			var userId = HttpContext.User.FindFirst(ClaimTypes.Sid)?.Value;
			if (userId is null) //eğer sid değeri cookie içinde bozulursa oturumu kapat
			{
				HttpContext.SignOutAsync(); //oturum kapat
				return RedirectToAction("Login", "Account"); //logine yönlendir
			}
			var list = _context.Employees
				.Select(c => new EmplooyeeSelectDto //dto lar veritabanındaki veriler kullanılacağı yerdeki nesneye dönüştürmemizi sağlar.
				{
					//böylece rezervationda customer lara gerek kalmaz
					Id = c.Id,
					Name = "Uzm. Dr. " + c.Name + " " + c.Surname
				}).ToList();
			ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Name");
			var model = new Rezervation();
			model.User = _context.Users.Find(Convert.ToInt32(userId)); //cookie string bir değer olduğundan int olarak alma kuralı geçerli durumunda hataya takılıyor, çözümü alırken int e çevirmek
			model.StartDate = DateTime.Now;
			model.EndDate = DateTime.Now.AddHours(1);
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Index(Rezervation rezervation)
		{
			var userId = HttpContext.User.FindFirst(ClaimTypes.Sid)?.Value;
			if (userId is null) //eğer sid değeri cookie içinde bozulursa oturumu kapat
			{
				await HttpContext.SignOutAsync(); //oturum kapat
				return RedirectToAction("Login", "Account"); //logine yönlendir
			}
			rezervation.UserId = Convert.ToInt32(userId);
			if (ModelState.IsValid)
			{
				try
				{
					await _context.AddAsync(rezervation);
					await _context.SaveChangesAsync();
					TempData["Message"] = @"<div class=""alert alert-success alert-dismissible fade show"" role=""alert"">
                     <strong>Rezervasyon İşlemi Başarılı! Lütfen saatinizi takip ediniz! Görüşmek üzere ^_^</strong>
                     <button type=""button"" class=""btn-close"" data-bs-dismiss=""alert"" aria-label=""Close""></button>
                     </div>";

					return RedirectToAction("Index", "Account");
				}
				catch (Exception)
				{
					ModelState.AddModelError("", "Rezervasyon sırasında bir hata oluştu!");
				}

			}
			var list = _context.Employees
				.Select(c => new EmplooyeeSelectDto
				{
					Id = c.Id,
					Name = "Uzm. Dr. " + c.Name + " " + c.Surname
				}).ToList();
			ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Email", rezervation.EmployeeId);
			return View(rezervation);
		}
	}
}
