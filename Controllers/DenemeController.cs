using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Migfer_Kariyer.Data;
using Migfer_Kariyer.Entities;
using Migfer_Kariyer.Models;

namespace Migfer_Kariyer.Controllers
{
    public class DenemeController : Controller
    {
        private readonly ApplicationContext _context;

        public DenemeController(ApplicationContext context)
        {
            _context = context;
        }
        public IActionResult Index2()
        {
            return View();
        }

        // Tablo görünümü için liste sayfası
        public IActionResult List()
        {
            var instructors = _context.Instructors
                .Include(i => i.InstructorFieldofSpecializations)
                .ThenInclude(ifs => ifs.FieldofSpecialization)
                .ToList();

            return View(instructors);
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new InstructorCreateViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(InstructorCreateViewModel model)
        {
            // Sadece dinamik alanları topla
            var allNewFieldNames = new List<string>();

            // Dinamik alanları ekle
            if (model.DynamicFields != null)
            {
                allNewFieldNames.AddRange(model.DynamicFields.Where(s => !string.IsNullOrWhiteSpace(s)));
            }

            // Boş alan varsa temizle ve duplicate'ları kaldır
            allNewFieldNames = allNewFieldNames
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            // Eğer hiç uzmanlık alanı girilmemişse hata ver
            if (!allNewFieldNames.Any())
            {
                ModelState.AddModelError("", "En az bir uzmanlık alanı eklemeniz gerekiyor. 'Yeni Alan Ekle' butonunu kullanarak uzmanlık alanları ekleyin.");
                return View(model);
            }

            // Diğer validation'ları kontrol et
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Yeni uzmanlık alanlarını veritabanına ekle
            var addedFields = new List<FieldofSpecialization>();

            foreach (var fieldName in allNewFieldNames)
            {
                // Aynı isimde alan var mı kontrol et
                var existingField = _context.FieldofSpecializations
                    .FirstOrDefault(f => f.Name.ToLower() == fieldName.ToLower());

                if (existingField == null)
                {
                    // Yeni alan oluştur
                    var newField = new FieldofSpecialization { Name = fieldName };
                    _context.FieldofSpecializations.Add(newField);
                    addedFields.Add(newField);
                }
                else
                {
                    // Mevcut alanı kullan
                    addedFields.Add(existingField);
                }
            }

            // Değişiklikleri kaydet (yeni alanlar için ID'ler oluşsun)
            _context.SaveChanges();

            // Eğitmeni oluştur
            var instructor = new Instructor
            {
                NameSurname = model.NameSurname,
                StudentCount = model.StudentCount,
                Experience = model.Experience,
                PhoneNumber = model.PhoneNumber,
                Email=model.Email,
                InstructorFieldofSpecializations = new List<InstructorFieldofSpecialization>()
            };

            // Tüm alanları eğitmene bağla
            foreach (var field in addedFields)
            {
                instructor.InstructorFieldofSpecializations.Add(new InstructorFieldofSpecialization
                {
                    FieldofSpecializationId = field.FieldofSpecializationId
                });
            }

            _context.Instructors.Add(instructor);
            _context.SaveChanges();

            // Başarı mesajı ile liste sayfasına yönlendir
            TempData["SuccessMessage"] = $"Eğitmen başarıyla kaydedildi! {addedFields.Count} uzmanlık alanı eklendi.";
            return RedirectToAction("List");
        }

        // Eğitmen silme
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var instructor = _context.Instructors
                .Include(i => i.InstructorFieldofSpecializations)
                .FirstOrDefault(i => i.InstructorId == id);

            if (instructor != null)
            {
                _context.Instructors.Remove(instructor);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Eğitmen başarıyla silindi.";
            }

            return RedirectToAction("List");
        }

        public IActionResult Edit(int id)
        {
            var instructor = _context.Instructors
                .Include(i => i.InstructorFieldofSpecializations)
                .ThenInclude(ifs => ifs.FieldofSpecialization)
                .FirstOrDefault(i => i.InstructorId == id);

            if (instructor == null)
                return NotFound();

            var viewModel = new InstructorCreateViewModel
            {
                NameSurname = instructor.NameSurname,
                StudentCount = instructor.StudentCount,
                Experience = instructor.Experience,
                Email = instructor.Email,
                PhoneNumber = instructor.PhoneNumber,
                IsActive = instructor.IsActive, // 👈 BURASI ÖNEMLİ!
                DynamicFields = instructor.InstructorFieldofSpecializations
                    .Select(x => x.FieldofSpecialization?.Name ?? "")
                    .ToList()
            };

            ViewBag.InstructorId = instructor.InstructorId;

            return View(viewModel);
        }


        // Eğitmen güncelleme
        [HttpPost]
        public IActionResult Edit(int id, InstructorCreateViewModel model)
        {
            var instructor = _context.Instructors
                .Include(i => i.InstructorFieldofSpecializations)
                .FirstOrDefault(i => i.InstructorId == id);

            if (instructor == null)
            {
                return NotFound();
            }

            // Sadece dinamik alanları işle
            var allNewFieldNames = new List<string>();

            if (model.DynamicFields != null)
            {
                allNewFieldNames.AddRange(model.DynamicFields.Where(s => !string.IsNullOrWhiteSpace(s)));
            }

            allNewFieldNames = allNewFieldNames
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            // Diğer validation'ları kontrol et
            if (!ModelState.IsValid)
            {
                ViewBag.InstructorId = id;
                return View("Index", model);
            }

            // Temel bilgileri güncelle
            instructor.NameSurname = model.NameSurname;
            instructor.StudentCount = model.StudentCount;
            instructor.Experience = model.Experience;
            instructor.IsActive = model.IsActive;

            // Mevcut uzmanlık alanlarını sil
            _context.InstructorFieldofSpecializations
                .RemoveRange(instructor.InstructorFieldofSpecializations);

            if (allNewFieldNames.Any())
            {
                var addedFields = new List<FieldofSpecialization>();

                foreach (var fieldName in allNewFieldNames)
                {
                    var existingField = _context.FieldofSpecializations
                        .FirstOrDefault(f => f.Name.ToLower() == fieldName.ToLower());

                    if (existingField == null)
                    {
                        var newField = new FieldofSpecialization { Name = fieldName };
                        _context.FieldofSpecializations.Add(newField);
                        addedFields.Add(newField);
                    }
                    else
                    {
                        addedFields.Add(existingField);
                    }
                }

                _context.SaveChanges();

                // Yeni alanları eğitmene bağla
                foreach (var field in addedFields)
                {
                    instructor.InstructorFieldofSpecializations.Add(new InstructorFieldofSpecialization
                    {
                        InstructorId = id,
                        FieldofSpecializationId = field.FieldofSpecializationId
                    });
                }
            }

            _context.SaveChanges();
            TempData["SuccessMessage"] = "Eğitmen başarıyla güncellendi.";
            return RedirectToAction("List");
        }
    }
}