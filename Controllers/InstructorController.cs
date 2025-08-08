using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Migfer_Kariyer.Data;
using Migfer_Kariyer.Entities;

namespace Migfer_Kariyer.Controllers
{
    public class InstructorController : Controller
    {
        private readonly ApplicationContext _context;

        public InstructorController(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var instructors = await _context.Instructors
                .Include(i => i.InstructorFieldofSpecializations)
                .ThenInclude(ifs => ifs.FieldofSpecialization)
                .OrderBy(i => i.NameSurname)
                .ToListAsync();

            return View(instructors);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var instructor = await _context.Instructors
                .Include(i => i.InstructorFieldofSpecializations)
                .ThenInclude(ifs => ifs.FieldofSpecialization)
                .Include(i => i.Educations)
                .FirstOrDefaultAsync(m => m.InstructorId == id);

            if (instructor == null) return NotFound();

            return View(instructor);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Specializations = await _context.FieldofSpecializations.ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Instructor instructor, List<int> selectedSpecializations)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using var transaction = await _context.Database.BeginTransactionAsync();

                    _context.Instructors.Add(instructor);
                    await _context.SaveChangesAsync();

                    if (selectedSpecializations != null && selectedSpecializations.Any())
                    {
                        foreach (var specId in selectedSpecializations)
                        {
                            var instructorSpec = new InstructorFieldofSpecialization
                            {
                                InstructorId = instructor.InstructorId,
                                FieldofSpecializationId = specId
                            };
                            _context.InstructorFieldofSpecializations.Add(instructorSpec);
                        }
                        await _context.SaveChangesAsync();
                    }

                    await transaction.CommitAsync();
                    TempData["Success"] = "Eğitmen başarıyla eklendi.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Kayıt sırasında hata: " + ex.Message);
                }
            }

            ViewBag.Specializations = await _context.FieldofSpecializations.ToListAsync();
            return View(instructor);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var instructor = await _context.Instructors
                .Include(i => i.InstructorFieldofSpecializations)
                .FirstOrDefaultAsync(i => i.InstructorId == id);

            if (instructor == null) return NotFound();

            ViewBag.Specializations = await _context.FieldofSpecializations.ToListAsync();
            ViewBag.SelectedSpecs = instructor.InstructorFieldofSpecializations.Select(x => x.FieldofSpecializationId).ToList();

            return View(instructor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Instructor instructor, List<int> selectedSpecializations)
        {
            if (id != instructor.InstructorId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    using var transaction = await _context.Database.BeginTransactionAsync();

                    // Eski uzmanlık alanlarını temizle
                    var existingSpecs = await _context.InstructorFieldofSpecializations
                        .Where(x => x.InstructorId == instructor.InstructorId)
                        .ToListAsync();

                    _context.InstructorFieldofSpecializations.RemoveRange(existingSpecs);

                    // Eğitmeni güncelle
                    _context.Update(instructor);

                    // Yeni uzmanlık alanlarını ekle
                    if (selectedSpecializations != null && selectedSpecializations.Any())
                    {
                        foreach (var specId in selectedSpecializations)
                        {
                            var instructorSpec = new InstructorFieldofSpecialization
                            {
                                InstructorId = instructor.InstructorId,
                                FieldofSpecializationId = specId
                            };
                            _context.InstructorFieldofSpecializations.Add(instructorSpec);
                        }
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    TempData["Success"] = "Eğitmen başarıyla güncellendi.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Güncelleme hatası: " + ex.Message);
                }
            }

            ViewBag.Specializations = await _context.FieldofSpecializations.ToListAsync();
            ViewBag.SelectedSpecs = selectedSpecializations ?? new List<int>();
            return View(instructor);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var instructor = await _context.Instructors
                .Include(i => i.InstructorFieldofSpecializations)
                .ThenInclude(ifs => ifs.FieldofSpecialization)
                .FirstOrDefaultAsync(m => m.InstructorId == id);

            if (instructor == null) return NotFound();

            return View(instructor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var instructor = await _context.Instructors
                    .Include(i => i.InstructorFieldofSpecializations)
                    .FirstOrDefaultAsync(i => i.InstructorId == id);

                if (instructor != null)
                {
                    _context.Instructors.Remove(instructor);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Eğitmen başarıyla silindi.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Silme hatası: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
