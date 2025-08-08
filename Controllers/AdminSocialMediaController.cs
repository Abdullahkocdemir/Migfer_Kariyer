using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Migfer_Kariyer.Data;
using Migfer_Kariyer.Entities;

namespace Migfer_Kariyer.Controllers
{
    public class AdminSocialMediaController : Controller
    {
        private readonly ApplicationContext _context;

        public AdminSocialMediaController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: AdminSocialMedia/Index
        public async Task<IActionResult> Index()
        {
            try
            {
                var socialMedias = await _context.SocialMedias.ToListAsync();

                // Debug için
                foreach (var item in socialMedias)
                {
                    Console.WriteLine($"ID: {item.Id}, Facebook: {item.FacebookUrl}, Twitter: {item.TwitterUrl}");
                }

                return View(socialMedias);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Index Hatası: {ex.Message}");
                return View(new List<SocialMedia>());
            }
        }

        // GET: AdminSocialMedia/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var socialMedia = await _context.SocialMedias
                .FirstOrDefaultAsync(m => m.Id == id);

            if (socialMedia == null)
            {
                return NotFound();
            }

            return View(socialMedia);
        }

        // GET: AdminSocialMedia/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminSocialMedia/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SocialMedia model)
        {
            try
            {
                // Debug için
                Console.WriteLine($"CREATE - Gelen Facebook: '{model.FacebookUrl}'");
                Console.WriteLine($"CREATE - Gelen Twitter: '{model.TwitterUrl}'");

                if (ModelState.IsValid)
                {
                    _context.SocialMedias.Add(model);
                    await _context.SaveChangesAsync();

                    // Kayıt sonrası kontrol
                    var savedItem = await _context.SocialMedias.FindAsync(model.Id);
                    Console.WriteLine($"CREATE - Kaydedilen Facebook: '{savedItem?.FacebookUrl}'");

                    TempData["SuccessMessage"] = "Sosyal medya bilgileri başarıyla eklendi!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    Console.WriteLine("CREATE - ModelState geçersiz:");
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        Console.WriteLine($"- {error.ErrorMessage}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CREATE Hatası: {ex.Message}");
                ModelState.AddModelError("", "Kayıt sırasında bir hata oluştu: " + ex.Message);
            }

            return View(model);
        }

        // GET: AdminSocialMedia/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var socialMedia = await _context.SocialMedias.FindAsync(id);
            if (socialMedia == null)
            {
                return NotFound();
            }

            // Debug için
            Console.WriteLine($"EDIT GET - Facebook: '{socialMedia.FacebookUrl}'");

            return View(socialMedia);
        }

        // POST: AdminSocialMedia/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SocialMedia model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            // Debug için
            Console.WriteLine($"EDIT POST - Gelen Facebook: '{model.FacebookUrl}'");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();

                    // Güncelleme sonrası kontrol
                    var updatedItem = await _context.SocialMedias.FindAsync(model.Id);
                    Console.WriteLine($"EDIT - Güncellenen Facebook: '{updatedItem?.FacebookUrl}'");

                    TempData["SuccessMessage"] = "Sosyal medya bilgileri başarıyla güncellendi!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SocialMediaExists(model.Id))
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
                    Console.WriteLine($"EDIT Hatası: {ex.Message}");
                    ModelState.AddModelError("", "Güncelleme sırasında bir hata oluştu: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("EDIT - ModelState geçersiz:");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"- {error.ErrorMessage}");
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
                var socialMedia = await _context.SocialMedias.FindAsync(id);

                if (socialMedia != null)
                {
                    Console.WriteLine($"DELETE - Silinen Facebook: '{socialMedia.FacebookUrl}'");
                    _context.SocialMedias.Remove(socialMedia);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Sosyal medya bilgileri başarıyla silindi!";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DELETE Hatası: {ex.Message}");
                TempData["ErrorMessage"] = "Silme sırasında bir hata oluştu: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        private bool SocialMediaExists(int id)
        {
            return _context.SocialMedias.Any(e => e.Id == id);
        }
    }
}