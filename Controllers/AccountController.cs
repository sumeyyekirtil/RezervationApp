using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RezervationApp.Data;
using RezervationApp.Entities;
using System.Security.Claims;

namespace RezervationApp.Controllers
{
	public class AccountController : Controller
	{
		private readonly DatabaseContext _context;

		public AccountController(DatabaseContext context)
		{
			_context = context;
		}

		[Authorize]
		public IActionResult Index()
		{
			var userId = HttpContext.User.FindFirst(ClaimTypes.Sid)?.Value;
			if (userId is null) //eğer sid değeri cookie içinde bozulursa oturumu kapat
			{
				HttpContext.SignOutAsync(); //oturum kapat
				return RedirectToAction("Login", "Account"); //logine yönlendir
			}
			var model = _context.Users.Find(Convert.ToInt32(userId));
			if (model == null)
			{
				HttpContext.SignOutAsync(); //oturum kapat
				return RedirectToAction("Login", "Account"); //logine yönlendir
			}
			return View(model);
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> Index(User user)
		{
			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(user);
					await _context.SaveChangesAsync();
				}
				catch
				{
					ModelState.AddModelError("", "Kayıt sırasında bir hata oluştu!");
				}
				return RedirectToAction(nameof(Index));
			}
			return View();
		}

		public IActionResult Login()
		{

			return View();
		}


		[HttpPost]
		public IActionResult Login(string email, string password, string ReturnUrl)
		{
			// Kullanıcı doğrulama işlemleri burada yapılacak (veritabanı kontrolü)
			//var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
			var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
			if (user != null)
			{
				// Giriş başarılı, kullanıcıyı yönlendir
				var haklar = new List<Claim>() //kullanıcı hakları tanımladık
				{
					new(ClaimTypes.Name, user.Name), //claim = hak (kullanıcıya tanımlanan haklar)
					new(ClaimTypes.Email, user.Email),
					new(ClaimTypes.Sid, user.Id.ToString()), //id yi string e çevirip kullandık
					new(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User"), //giriş yapan kullanıcı admin yetkisiyle değilse user yetkisiyle giriş yapsın.
					new(ClaimTypes.UserData, user.UserGuid.Value.ToString()),
				};
				var kullaniciKimligi = new ClaimsIdentity(haklar, "Login"); //kullanıcı için bir kimlik oluşturduk
				ClaimsPrincipal claimsPrincipal = new(kullaniciKimligi); //bu sınıftan bir nesne oluşturup bilgilerde saklı haklar ile kural oluşturulabilir
				HttpContext.SignInAsync(claimsPrincipal); //yukarıdaki yetkilerle sisteme giriş yaptık
				if (!string.IsNullOrEmpty(ReturnUrl))
				{
					return Redirect(ReturnUrl);
				}
				return RedirectToAction("Index", "Home");
			}
			else
			{
				//Giriş başarısız, hata mesajı göster
				//ViewBag.ErrorMessage = "Geçersiz e-posta veya şifre."; //2.yol
				TempData["Message"] = @"<div class=""alert alert-danger alert-dismissible fade show"" role=""alert"">
                     <strong>Giriş Başarısız! Lütfen bilgilerinizin doğruluğunu kontrol ediniz!!!</strong>
                     <button type=""button"" class=""btn-close"" data-bs-dismiss=""alert"" aria-label=""Close""></button>
                     </div>"; ;
			}
			return RedirectToAction("Login", "Account");
		}

		public ActionResult LogOut() //çıkış yap aktivasyonu : layout
		{
			HttpContext.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}

		public IActionResult Register()
		{
			return View();
		}

		public IActionResult AccessDenied() //varsayılan metod yolu açtık
		{
			return View();
		}

		[HttpPost]
		public IActionResult Register(User user)
		{
			if (ModelState.IsValid)
			{
				try
				{
					user.IsActive = true;
					user.IsAdmin = false;
					user.UserType = UserType.Customer; //userType ı customer yani müşteri olarak ayarla (kayıt işleminde)
					_context.Users.Add(user);
					_context.SaveChanges();
					//_userService.AddUser(user);
					//_userService.Save();
					TempData["Message"] = @"<div class=""alert alert-success alert-dismissible fade show"" role=""alert"">
                     <strong>Kayıt İşlemi Başarılı! Giriş Yapabilirsiniz.!</strong>
                     <button type=""button"" class=""btn-close"" data-bs-dismiss=""alert"" aria-label=""Close""></button>
                     </div>";
					return RedirectToAction("Login", "Account");
				}
				catch (Exception)
				{
					ModelState.AddModelError("", "Kayıt sırasında bir hata oluştu!");
				}
			}
			return View(user);
		}
	}
}
