using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Migfer_Kariyer.Data;
using Migfer_Kariyer.Entities;

namespace Migfer_Kariyer.Controllers
{
    public class AdminInstructorController : Controller
    {
        private readonly ApplicationContext _context;

        public AdminInstructorController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: AdminInstructor/Index
        public async Task<IActionResult> Index()
        {
            try
            {
                var instructors = await _context.Instructors
                    .Include(i => i.InstructorFieldofSpecializations)
                    .ThenInclude(ifs => ifs.FieldofSpecialization)
                    .Include(i => i.Educations)
                    .OrderBy(i => i.NameSurname)
                    .ToListAsync();

                // Debug için
                foreach (var instructor in instructors)
                {
                    Console.WriteLine($"ID: {instructor.InstructorId}, Name: {instructor.NameSurname}, " +
                                    $"Specializations: {instructor.InstructorFieldofSpecializations.Count}, " +
                                    $"Educations: {instructor.Educations.Count}, Active: {instructor.IsActive}");
                }

                return View(instructors);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Index Hatası: {ex.Message}");
                return View(new List<Instructor>());
            }
        }

        // GET: AdminInstructor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .Include(i => i.InstructorFieldofSpecializations)
                .ThenInclude(ifs => ifs.FieldofSpecialization)
                .Include(i => i.Educations)
                .FirstOrDefaultAsync(m => m.InstructorId == id);

            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // GET: AdminInstructor/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                // Artık mevcut uzmanlık alanlarını ViewBag ile göndermiyoruz
                // Çünkü kullanıcı kendisi yazacak
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Create GET Hatası: {ex.Message}");
                return View();
            }
        }

        // POST: AdminInstructor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Instructor instructor, List<string> SpecializationFields)
        {
            try
            {
                // Debug için
                Console.WriteLine($"CREATE - Gelen Name: '{instructor.NameSurname}'");
                Console.WriteLine($"CREATE - Gelen Email: '{instructor.Email}'");
                Console.WriteLine($"CREATE - Specialization Fields: {string.Join(",", SpecializationFields ?? new List<string>())}");

                if (ModelState.IsValid)
                {
                    using var transaction = await _context.Database.BeginTransactionAsync();

                    // Eğitmeni ekle
                    _context.Instructors.Add(instructor);
                    await _context.SaveChangesAsync();

                    // Uzmanlık alanlarını işle
                    if (SpecializationFields != null && SpecializationFields.Any())
                    {
                        foreach (var specName in SpecializationFields)
                        {
                            if (!string.IsNullOrWhiteSpace(specName))
                            {
                                var trimmedName = specName.Trim();

                                // Bu uzmanlık alanı daha önce var mı kontrol et
                                var existingSpec = await _context.FieldofSpecializations
                                    .FirstOrDefaultAsync(f => f.Name.ToLower() == trimmedName.ToLower());

                                if (existingSpec == null)
                                {
                                    // Yoksa yeni oluştur
                                    existingSpec = new FieldofSpecialization
                                    {
                                        Name = trimmedName
                                    };
                                    _context.FieldofSpecializations.Add(existingSpec);
                                    await _context.SaveChangesAsync();
                                    Console.WriteLine($"CREATE - Yeni uzmanlık alanı oluşturuldu: '{trimmedName}'");
                                }
                                else
                                {
                                    Console.WriteLine($"CREATE - Mevcut uzmanlık alanı kullanıldı: '{existingSpec.Name}'");
                                }

                                // Eğitmen-Uzmanlık ilişkisini oluştur
                                var instructorSpec = new InstructorFieldofSpecialization
                                {
                                    InstructorId = instructor.InstructorId,
                                    FieldofSpecializationId = existingSpec.FieldofSpecializationId
                                };
                                _context.InstructorFieldofSpecializations.Add(instructorSpec);
                            }
                        }
                        await _context.SaveChangesAsync();
                    }

                    await transaction.CommitAsync();

                    // Kayıt sonrası kontrol
                    var savedInstructor = await _context.Instructors
                        .Include(i => i.InstructorFieldofSpecializations)
                        .ThenInclude(ifs => ifs.FieldofSpecialization)
                        .FirstOrDefaultAsync(i => i.InstructorId == instructor.InstructorId);

                    Console.WriteLine($"CREATE - Kaydedilen Name: '{savedInstructor?.NameSurname}'");
                    Console.WriteLine($"CREATE - Kaydedilen Specializations: {savedInstructor?.InstructorFieldofSpecializations.Count}");

                    foreach (var spec in savedInstructor?.InstructorFieldofSpecializations ?? new List<InstructorFieldofSpecialization>())
                    {
                        Console.WriteLine($"  - {spec.FieldofSpecialization?.Name}");
                    }

                    TempData["SuccessMessage"] = "Eğitmen başarıyla eklendi!";
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
                Console.WriteLine($"CREATE Stack Trace: {ex.StackTrace}");
                ModelState.AddModelError("", "Kayıt sırasında bir hata oluştu: " + ex.Message);
            }

            // Hata durumunda form'u tekrar göster
            ViewBag.SpecializationFields = SpecializationFields ?? new List<string>();
            return View(instructor);
        }

        // GET: AdminInstructor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .Include(i => i.InstructorFieldofSpecializations)
                .ThenInclude(ifs => ifs.FieldofSpecialization)
                .FirstOrDefaultAsync(i => i.InstructorId == id);

            if (instructor == null)
            {
                return NotFound();
            }

            // Debug için
            Console.WriteLine($"EDIT GET - Name: '{instructor.NameSurname}'");
            Console.WriteLine($"EDIT GET - Current Specializations: {instructor.InstructorFieldofSpecializations.Count}");

            foreach (var spec in instructor.InstructorFieldofSpecializations)
            {
                Console.WriteLine($"  - {spec.FieldofSpecialization?.Name}");
            }

            return View(instructor);
        }

        // POST: AdminInstructor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Instructor instructor, List<string> SpecializationFields)
        {
            if (id != instructor.InstructorId)
            {
                return NotFound();
            }

            // Debug için
            Console.WriteLine($"EDIT POST - Gelen Name: '{instructor.NameSurname}'");
            Console.WriteLine($"EDIT POST - Specialization Fields: {string.Join(",", SpecializationFields ?? new List<string>())}");

            if (ModelState.IsValid)
            {
                try
                {
                    using var transaction = await _context.Database.BeginTransactionAsync();

                    // Eski uzmanlık alanlarını temizle
                    var existingSpecs = await _context.InstructorFieldofSpecializations
                        .Where(x => x.InstructorId == instructor.InstructorId)
                        .ToListAsync();

                    Console.WriteLine($"EDIT - Silinen eski uzmanlık sayısı: {existingSpecs.Count}");
                    _context.InstructorFieldofSpecializations.RemoveRange(existingSpecs);

                    // Eğitmeni güncelle
                    _context.Update(instructor);

                    // Yeni uzmanlık alanlarını işle
                    if (SpecializationFields != null && SpecializationFields.Any())
                    {
                        foreach (var specName in SpecializationFields)
                        {
                            if (!string.IsNullOrWhiteSpace(specName))
                            {
                                var trimmedName = specName.Trim();

                                // Bu uzmanlık alanı daha önce var mı kontrol et
                                var existingSpec = await _context.FieldofSpecializations
                                    .FirstOrDefaultAsync(f => f.Name.ToLower() == trimmedName.ToLower());

                                if (existingSpec == null)
                                {
                                    // Yoksa yeni oluştur
                                    existingSpec = new FieldofSpecialization
                                    {
                                        Name = trimmedName
                                    };
                                    _context.FieldofSpecializations.Add(existingSpec);
                                    await _context.SaveChangesAsync();
                                    Console.WriteLine($"EDIT - Yeni uzmanlık alanı oluşturuldu: '{trimmedName}'");
                                }
                                else
                                {
                                    Console.WriteLine($"EDIT - Mevcut uzmanlık alanı kullanıldı: '{existingSpec.Name}'");
                                }

                                // Eğitmen-Uzmanlık ilişkisini oluştur
                                var instructorSpec = new InstructorFieldofSpecialization
                                {
                                    InstructorId = instructor.InstructorId,
                                    FieldofSpecializationId = existingSpec.FieldofSpecializationId
                                };
                                _context.InstructorFieldofSpecializations.Add(instructorSpec);
                            }
                        }
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    // Güncelleme sonrası kontrol
                    var updatedInstructor = await _context.Instructors
                        .Include(i => i.InstructorFieldofSpecializations)
                        .ThenInclude(ifs => ifs.FieldofSpecialization)
                        .FirstOrDefaultAsync(i => i.InstructorId == instructor.InstructorId);

                    Console.WriteLine($"EDIT - Güncellenen Name: '{updatedInstructor?.NameSurname}'");
                    Console.WriteLine($"EDIT - Güncellenen Specializations: {updatedInstructor?.InstructorFieldofSpecializations.Count}");

                    foreach (var spec in updatedInstructor?.InstructorFieldofSpecializations ?? new List<InstructorFieldofSpecialization>())
                    {
                        Console.WriteLine($"  - {spec.FieldofSpecialization?.Name}");
                    }

                    TempData["SuccessMessage"] = "Eğitmen başarıyla güncellendi!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstructorExists(instructor.InstructorId))
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
                    Console.WriteLine($"EDIT Stack Trace: {ex.StackTrace}");
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

            // Hata durumunda mevcut instructor'ı tekrar yükle
            var instructorForView = await _context.Instructors
                .Include(i => i.InstructorFieldofSpecializations)
                .ThenInclude(ifs => ifs.FieldofSpecialization)
                .FirstOrDefaultAsync(i => i.InstructorId == instructor.InstructorId);

            ViewBag.SpecializationFields = SpecializationFields ?? new List<string>();
            return View(instructorForView ?? instructor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var instructor = await _context.Instructors
                    .Include(i => i.InstructorFieldofSpecializations)
                    .Include(i => i.Educations)
                    .FirstOrDefaultAsync(i => i.InstructorId == id);

                if (instructor != null)
                {
                    // Eğer bu eğitmenin verdiği eğitimler varsa uyarı ver
                    if (instructor.Educations.Any())
                    {
                        TempData["ErrorMessage"] = $"Bu eğitmen {instructor.Educations.Count} eğitim veriyor. Önce o eğitimleri başka eğitmene atayın veya silin.";
                        return RedirectToAction(nameof(Index));
                    }

                    Console.WriteLine($"DELETE - Silinen Name: '{instructor.NameSurname}'");
                    Console.WriteLine($"DELETE - Silinen Specializations: {instructor.InstructorFieldofSpecializations.Count}");

                    // Entity Framework Cascade Delete ile uzmanlık ilişkileri otomatik silinecek
                    _context.Instructors.Remove(instructor);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Eğitmen başarıyla silindi!";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DELETE Hatası: {ex.Message}");
                TempData["ErrorMessage"] = "Silme sırasında bir hata oluştu: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: AdminInstructor/ToggleStatus/5
        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            try
            {
                var instructor = await _context.Instructors.FindAsync(id);
                if (instructor != null)
                {
                    instructor.IsActive = !instructor.IsActive;
                    await _context.SaveChangesAsync();

                    var status = instructor.IsActive ? "aktif" : "pasif";
                    TempData["SuccessMessage"] = $"Eğitmen durumu {status} olarak güncellendi!";

                    Console.WriteLine($"TOGGLE - {instructor.NameSurname} durumu {status} yapıldı");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ToggleStatus Hatası: {ex.Message}");
                TempData["ErrorMessage"] = "Durum güncelleme sırasında bir hata oluştu: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        private bool InstructorExists(int id)
        {
            return _context.Instructors.Any(e => e.InstructorId == id);
        }
    }
}