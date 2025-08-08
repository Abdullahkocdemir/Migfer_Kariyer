using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Migfer_Kariyer.Data;
using Migfer_Kariyer.Entities;

namespace Migfer_Kariyer.Controllers
{
    public class AdminContactTextController : Controller
    {
        private readonly ApplicationContext _context;

        public AdminContactTextController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: AdminContactText/Index
        public async Task<IActionResult> Index()
        {
            try
            {
                var contactTexts = await _context.ContactTexts.ToListAsync();
                return View(contactTexts);
            }
            catch (Exception ex)
            {
                // Hata durumunda boş liste döndür
                return View(new List<ContactText>());
            }
        }

        // GET: AdminContactText/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactText = await _context.ContactTexts
                .FirstOrDefaultAsync(m => m.Id == id);

            if (contactText == null)
            {
                return NotFound();
            }

            return View(contactText);
        }

        // GET: AdminContactText/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminContactText/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContactText model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.ContactTexts.Add(model);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "İletişim bilgileri başarıyla eklendi!";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Kayıt sırasında bir hata oluştu: " + ex.Message);
            }

            return View(model);
        }

        // GET: AdminContactText/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactText = await _context.ContactTexts.FindAsync(id);
            if (contactText == null)
            {
                return NotFound();
            }

            return View(contactText);
        }

        // POST: AdminContactText/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ContactText model)
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

                    TempData["SuccessMessage"] = "İletişim bilgileri başarıyla güncellendi!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactTextExists(model.Id))
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
                var contactText = await _context.ContactTexts.FindAsync(id);
                if (contactText != null)
                {
                    _context.ContactTexts.Remove(contactText);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "İletişim bilgileri başarıyla silindi!";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Silme sırasında bir hata oluştu: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ContactTextExists(int id)
        {
            return _context.ContactTexts.Any(e => e.Id == id);
        }
    }
}