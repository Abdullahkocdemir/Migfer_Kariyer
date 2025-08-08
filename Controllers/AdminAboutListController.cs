using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Migfer_Kariyer.Data;
using Migfer_Kariyer.Entities;

namespace Migfer_Kariyer.Controllers
{
    public class AdminAboutListController : Controller
    {
        private readonly ApplicationContext _context;

        public AdminAboutListController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: AdminAboutList/Index
        public async Task<IActionResult> Index()
        {
            try
            {
                var aboutLists = await _context.AboutLists.ToListAsync();

                // Debug için - Console'da ikonları kontrol et
                foreach (var item in aboutLists)
                {
                    Console.WriteLine($"ID: {item.Id}, Başlık: {item.Başlık}, İkon: '{item.Ikon}' (Uzunluk: {item.Ikon?.Length ?? 0})");
                }

                return View(aboutLists);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Index Hatası: {ex.Message}");
                return View(new List<AboutList>());
            }
        }

        // GET: AdminAboutList/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aboutList = await _context.AboutLists
                .FirstOrDefaultAsync(m => m.Id == id);

            if (aboutList == null)
            {
                return NotFound();
            }

            return View(aboutList);
        }

        // GET: AdminAboutList/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminAboutList/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AboutList model)
        {
            try
            {
                // Debug için - Gelen veriyi kontrol et
                Console.WriteLine($"CREATE - Gelen Başlık: '{model.Başlık}'");
                Console.WriteLine($"CREATE - Gelen İkon: '{model.Ikon}' (Uzunluk: {model.Ikon?.Length ?? 0})");
                Console.WriteLine($"CREATE - Gelen Açıklama: '{model.Açıklama?.Substring(0, Math.Min(50, model.Açıklama?.Length ?? 0))}...'");

                if (ModelState.IsValid)
                {
                    _context.AboutLists.Add(model);
                    await _context.SaveChangesAsync();

                    // Kayıt sonrası kontrol
                    var savedItem = await _context.AboutLists.FindAsync(model.Id);
                    Console.WriteLine($"CREATE - Kaydedilen İkon: '{savedItem?.Ikon}'");

                    TempData["SuccessMessage"] = "Hakkımızda bilgisi başarıyla eklendi!";
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

        // GET: AdminAboutList/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aboutList = await _context.AboutLists.FindAsync(id);
            if (aboutList == null)
            {
                return NotFound();
            }

            // Debug için
            Console.WriteLine($"EDIT GET - İkon: '{aboutList.Ikon}'");

            return View(aboutList);
        }

        // POST: AdminAboutList/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AboutList model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            // Debug için
            Console.WriteLine($"EDIT POST - Gelen İkon: '{model.Ikon}' (Uzunluk: {model.Ikon?.Length ?? 0})");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();

                    // Güncelleme sonrası kontrol
                    var updatedItem = await _context.AboutLists.FindAsync(model.Id);
                    Console.WriteLine($"EDIT - Güncellenen İkon: '{updatedItem?.Ikon}'");

                    TempData["SuccessMessage"] = "Hakkımızda bilgisi başarıyla güncellendi!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AboutListExists(model.Id))
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
                var aboutList = await _context.AboutLists.FindAsync(id);
                if (aboutList != null)
                {
                    Console.WriteLine($"DELETE - Silinen İkon: '{aboutList.Ikon}'");
                    _context.AboutLists.Remove(aboutList);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Hakkımızda bilgisi başarıyla silindi!";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DELETE Hatası: {ex.Message}");
                TempData["ErrorMessage"] = "Silme sırasında bir hata oluştu: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        private bool AboutListExists(int id)
        {
            return _context.AboutLists.Any(e => e.Id == id);
        }
    }
}