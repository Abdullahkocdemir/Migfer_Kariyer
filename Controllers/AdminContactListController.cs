using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Migfer_Kariyer.Data;
using Migfer_Kariyer.Entities;

namespace Migfer_Kariyer.Controllers
{
    public class AdminContactListController : Controller
    {
        private readonly ApplicationContext _context;

        public AdminContactListController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: AdminContactList/Index
        public async Task<IActionResult> Index()
        {
            try
            {
                var contactLists = await _context.ContactLists.ToListAsync();

                // Debug için - Console'da ikonları kontrol et
                foreach (var item in contactLists)
                {
                    Console.WriteLine($"ID: {item.Id}, Adı: {item.Adı}, İkon: '{item.İkon}' (Uzunluk: {item.İkon?.Length ?? 0})");
                }

                return View(contactLists);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Index Hatası: {ex.Message}");
                return View(new List<ContactList>());
            }
        }

        // GET: AdminContactList/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactList = await _context.ContactLists
                .FirstOrDefaultAsync(m => m.Id == id);

            if (contactList == null)
            {
                return NotFound();
            }

            return View(contactList);
        }

        // GET: AdminContactList/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminContactList/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContactList model)
        {
            try
            {
                // Debug için - Gelen veriyi kontrol et
                Console.WriteLine($"CREATE - Gelen Adı: '{model.Adı}'");
                Console.WriteLine($"CREATE - Gelen İkon: '{model.İkon}' (Uzunluk: {model.İkon?.Length ?? 0})");
                Console.WriteLine($"CREATE - Gelen Açıklama: '{model.Açıklama}'");

                // ModelState kontrolü
                if (!ModelState.IsValid)
                {
                    Console.WriteLine("ModelState geçersiz:");
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        Console.WriteLine($"- {error.ErrorMessage}");
                    }
                }

                if (ModelState.IsValid)
                {
                    _context.ContactLists.Add(model);
                    await _context.SaveChangesAsync();

                    // Kayıt sonrası kontrol
                    var savedItem = await _context.ContactLists.FindAsync(model.Id);
                    Console.WriteLine($"CREATE - Kaydedilen İkon: '{savedItem?.İkon}'");

                    TempData["SuccessMessage"] = "İletişim bilgisi başarıyla eklendi!";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CREATE Hatası: {ex.Message}");
                Console.WriteLine($"CREATE Stack Trace: {ex.StackTrace}");
                ModelState.AddModelError("", "Kayıt sırasında bir hata oluştu: " + ex.Message);
            }

            return View(model);
        }

        // GET: AdminContactList/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactList = await _context.ContactLists.FindAsync(id);
            if (contactList == null)
            {
                return NotFound();
            }

            // Debug için
            Console.WriteLine($"EDIT GET - İkon: '{contactList.İkon}'");

            return View(contactList);
        }

        // POST: AdminContactList/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ContactList model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            // Debug için
            Console.WriteLine($"EDIT POST - Gelen İkon: '{model.İkon}' (Uzunluk: {model.İkon?.Length ?? 0})");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();

                    // Güncelleme sonrası kontrol
                    var updatedItem = await _context.ContactLists.FindAsync(model.Id);
                    Console.WriteLine($"EDIT - Güncellenen İkon: '{updatedItem?.İkon}'");

                    TempData["SuccessMessage"] = "İletişim bilgisi başarıyla güncellendi!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactListExists(model.Id))
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
                var contactList = await _context.ContactLists.FindAsync(id);
                if (contactList != null)
                {
                    Console.WriteLine($"DELETE - Silinen İkon: '{contactList.İkon}'");
                    _context.ContactLists.Remove(contactList);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "İletişim bilgisi başarıyla silindi!";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DELETE Hatası: {ex.Message}");
                TempData["ErrorMessage"] = "Silme sırasında bir hata oluştu: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ContactListExists(int id)
        {
            return _context.ContactLists.Any(e => e.Id == id);
        }
    }
}