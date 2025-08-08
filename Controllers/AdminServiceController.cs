using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Migfer_Kariyer.Data;
using Migfer_Kariyer.Entities;

namespace Migfer_Kariyer.Controllers
{
    public class AdminServiceController : Controller
    {
        private readonly ApplicationContext _context;

        public AdminServiceController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: AdminService/Index
        public async Task<IActionResult> Index()
        {
            try
            {
                var services = await _context.Services
                    .Include(s => s.Maddelers)
                    .ToListAsync();
                return View(services);
            }
            catch (Exception ex)
            {
                // Hata durumunda boş liste döndür
                return View(new List<Service>());
            }
        }

        // GET: AdminService/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .Include(s => s.Maddelers)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // GET: AdminService/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminService/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Service model, List<string> MaddelerFields)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Service'i ekle
                    _context.Services.Add(model);
                    await _context.SaveChangesAsync();

                    // Maddeler'i ekle
                    if (MaddelerFields != null && MaddelerFields.Any())
                    {
                        foreach (var madde in MaddelerFields.Where(m => !string.IsNullOrWhiteSpace(m)))
                        {
                            var maddeler = new Maddeler
                            {
                                Name = madde.Trim(),
                                ServicesId = model.Id
                            };
                            _context.Maddelers.Add(maddeler);
                        }
                        await _context.SaveChangesAsync();
                    }

                    TempData["SuccessMessage"] = "Servis başarıyla eklendi!";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Kayıt sırasında bir hata oluştu: " + ex.Message);
            }

            return View(model);
        }

        // GET: AdminService/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .Include(s => s.Maddelers)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // POST: AdminService/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Service model, List<string> MaddelerFields)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Service'i güncelle
                    _context.Update(model);

                    // Mevcut maddeleri sil
                    var existingMaddeler = await _context.Maddelers
                        .Where(m => m.ServicesId == model.Id)
                        .ToListAsync();

                    _context.Maddelers.RemoveRange(existingMaddeler);

                    // Yeni maddeleri ekle
                    if (MaddelerFields != null && MaddelerFields.Any())
                    {
                        foreach (var madde in MaddelerFields.Where(m => !string.IsNullOrWhiteSpace(m)))
                        {
                            var maddeler = new Maddeler
                            {
                                Name = madde.Trim(),
                                ServicesId = model.Id
                            };
                            _context.Maddelers.Add(maddeler);
                        }
                    }

                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Servis başarıyla güncellendi!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Güncelleme sırasında bir hata oluştu: " + ex.Message);
                }
            }

            // Hata durumunda mevcut maddeleri yeniden yükle
            var serviceWithMaddeler = await _context.Services
                .Include(s => s.Maddelers)
                .FirstOrDefaultAsync(s => s.Id == id);

            return View(serviceWithMaddeler ?? model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var service = await _context.Services
                    .Include(s => s.Maddelers)
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (service != null)
                {
                    // Önce maddeleri sil
                    _context.Maddelers.RemoveRange(service.Maddelers);

                    // Sonra servisi sil
                    _context.Services.Remove(service);

                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Servis başarıyla silindi!";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Silme sırasında bir hata oluştu: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ServiceExists(int id)
        {
            return _context.Services.Any(e => e.Id == id);
        }
    }
}