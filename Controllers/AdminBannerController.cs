using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Migfer_Kariyer.Data;
using Migfer_Kariyer.Entities;

namespace Migfer_Kariyer.Controllers
{
    public class AdminBannerController : Controller
    {
        private readonly ApplicationContext _context;

        public AdminBannerController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: AdminBanner/Index
        public async Task<IActionResult> Index()
        {
            try
            {
                var banners = await _context.Banners.ToListAsync();
                return View(banners);
            }
            catch (Exception ex)
            {
                // Hata durumunda boş liste döndür
                return View(new List<Banner>());
            }
        }

        // GET: AdminBanner/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banner = await _context.Banners
                .FirstOrDefaultAsync(m => m.Id == id);

            if (banner == null)
            {
                return NotFound();
            }

            return View(banner);
        }

        // GET: AdminBanner/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminBanner/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Banner model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Banners.Add(model);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Banner başarıyla eklendi!";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Kayıt sırasında bir hata oluştu: " + ex.Message);
            }

            return View(model);
        }

        // GET: AdminBanner/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banner = await _context.Banners.FindAsync(id);
            if (banner == null)
            {
                return NotFound();
            }

            return View(banner);
        }

        // POST: AdminBanner/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Banner model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Banner başarıyla güncellendi!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BannerExists(model.Id))
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

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var banner = await _context.Banners.FindAsync(id);
                if (banner != null)
                {
                    _context.Banners.Remove(banner);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Banner başarıyla silindi!";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Silme sırasında bir hata oluştu: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        private bool BannerExists(int id)
        {
            return _context.Banners.Any(e => e.Id == id);
        }
    }
}