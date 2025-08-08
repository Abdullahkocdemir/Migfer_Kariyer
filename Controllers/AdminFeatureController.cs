using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Migfer_Kariyer.Data;
using Migfer_Kariyer.Entities;

namespace Migfer_Kariyer.Controllers
{
    public class AdminFeatureController : Controller
    {
        private readonly ApplicationContext _context;

        public AdminFeatureController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: AdminFeature/Index
        public async Task<IActionResult> Index()
        {
            try
            {
                var features = await _context.Features
                    .Include(f => f.Educations)
                    .ToListAsync();

                // Debug için
                foreach (var item in features)
                {
                    Console.WriteLine($"ID: {item.Id}, Name: {item.Name}, Education Count: {item.Educations.Count}");
                }

                return View(features);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Index Hatası: {ex.Message}");
                return View(new List<Feature>());
            }
        }

        // GET: AdminFeature/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feature = await _context.Features
                .Include(f => f.Educations)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (feature == null)
            {
                return NotFound();
            }

            return View(feature);
        }

        // GET: AdminFeature/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminFeature/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Feature model)
        {
            try
            {
                // Debug için
                Console.WriteLine($"CREATE - Gelen Name: '{model.Name}'");

                if (ModelState.IsValid)
                {
                    _context.Features.Add(model);
                    await _context.SaveChangesAsync();

                    // Kayıt sonrası kontrol
                    var savedItem = await _context.Features.FindAsync(model.Id);
                    Console.WriteLine($"CREATE - Kaydedilen Name: '{savedItem?.Name}'");

                    TempData["SuccessMessage"] = "Özellik başarıyla eklendi!";
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

        // GET: AdminFeature/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feature = await _context.Features.FindAsync(id);
            if (feature == null)
            {
                return NotFound();
            }

            // Debug için
            Console.WriteLine($"EDIT GET - Name: '{feature.Name}'");

            return View(feature);
        }

        // POST: AdminFeature/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Feature model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            // Debug için
            Console.WriteLine($"EDIT POST - Gelen Name: '{model.Name}'");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();

                    // Güncelleme sonrası kontrol
                    var updatedItem = await _context.Features.FindAsync(model.Id);
                    Console.WriteLine($"EDIT - Güncellenen Name: '{updatedItem?.Name}'");

                    TempData["SuccessMessage"] = "Özellik başarıyla güncellendi!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FeatureExists(model.Id))
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
                var feature = await _context.Features
                    .Include(f => f.Educations)
                    .FirstOrDefaultAsync(f => f.Id == id);

                if (feature != null)
                {
                    // Eğer bu özelliği kullanan eğitimler varsa uyarı ver
                    if (feature.Educations.Any())
                    {
                        TempData["ErrorMessage"] = $"Bu özellik {feature.Educations.Count} eğitim tarafından kullanılıyor. Önce o eğitimleri güncelleyin.";
                        return RedirectToAction(nameof(Index));
                    }

                    Console.WriteLine($"DELETE - Silinen Name: '{feature.Name}'");
                    _context.Features.Remove(feature);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Özellik başarıyla silindi!";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DELETE Hatası: {ex.Message}");
                TempData["ErrorMessage"] = "Silme sırasında bir hata oluştu: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        private bool FeatureExists(int id)
        {
            return _context.Features.Any(e => e.Id == id);
        }
    }
}