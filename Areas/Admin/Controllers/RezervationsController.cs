using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RezervationApp.Data;
using RezervationApp.Entities;

namespace RezervationApp.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class RezervationsController : Controller
    {
        private readonly DatabaseContext _context;

        public RezervationsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Admin/Rezervations
        public async Task<IActionResult> Index()
        {
            var databaseContext = _context.Rezervations.Include(r => r.Employee).Include(r => r.User);
            return View(await databaseContext.ToListAsync());
        }

        // GET: Admin/Rezervations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervation = await _context.Rezervations
                .Include(r => r.Employee)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rezervation == null)
            {
                return NotFound();
            }

            return View(rezervation);
        }

        // GET: Admin/Rezervations/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Email");
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Email");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            return View();
        }

        // POST: Admin/Rezervations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Rezervation rezervation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rezervation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Email", rezervation.EmployeeId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", rezervation.UserId);
            return View(rezervation);
        }

        // GET: Admin/Rezervations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervation = await _context.Rezervations.FindAsync(id);
            if (rezervation == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Email", rezervation.EmployeeId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", rezervation.UserId);
            return View(rezervation);
        }

        // POST: Admin/Rezervations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Rezervation rezervation)
        {
            if (id != rezervation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rezervation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RezervationExists(rezervation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Email", rezervation.EmployeeId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", rezervation.UserId);
            return View(rezervation);
        }

        // GET: Admin/Rezervations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervation = await _context.Rezervations
                .Include(r => r.Employee)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rezervation == null)
            {
                return NotFound();
            }

            return View(rezervation);
        }

        // POST: Admin/Rezervations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rezervation = await _context.Rezervations.FindAsync(id);
            if (rezervation != null)
            {
                _context.Rezervations.Remove(rezervation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RezervationExists(int id)
        {
            return _context.Rezervations.Any(e => e.Id == id);
        }
    }
}
