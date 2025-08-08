using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Migfer_Kariyer.Data;
using Migfer_Kariyer.Entities;
using Migfer_Kariyer.Models;
using System;

namespace Migfer_Kariyer.Controllers
{
    public class AdminEducationController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminEducationController(ApplicationContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: AdminEducation/Index
        public async Task<IActionResult> Index()
        {
            try
            {
                var educations = await _context.Educations
                    .Include(e => e.Instructor)
                    .Include(e => e.Feature)
                    .ToListAsync();

                return View(educations);
            }
            catch (Exception ex)
            {
                // Hata durumunda boş liste döndür
                return View(new List<Education>());
            }
        }

        // GET: AdminEducation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var education = await _context.Educations
                .Include(e => e.Instructor)
                .Include(e => e.Feature)
                .Include(e => e.WhatYouWillLearns)
                .Include(e => e.Requirements)
                .Include(e => e.CourseContents)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (education == null)
            {
                return NotFound();
            }

            return View(education);
        }

        // GET: AdminEducation/Create
        public async Task<IActionResult> Create()
        {
            await LoadSelectLists();
            var viewModel = new EducationCreateViewModel();
            return View(viewModel);
        }

        // POST: AdminEducation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EducationCreateViewModel model)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Fotoğraf yükleme işlemi
                string photoFileName = "";
                if (model.PhotoFile != null && model.PhotoFile.Length > 0)
                {
                    photoFileName = await SavePhotoAsync(model.PhotoFile);
                }

                // Education kaydını oluştur
                var education = new Education
                {
                    Name = model.Name,
                    ShortDescription = model.ShortDescription,
                    Description = model.LongDescription ?? "",
                    StudentCount = model.StudentCount,
                    CourseHours = model.CourseHours,
                    Language = model.Language,
                    PhotoUrl = photoFileName,
                    IsPopularCourse = model.IsPopularCourse,
                    IsActive = model.IsActive,
                    InstructorId = model.InstructorId,
                    FeatureId = model.FeatureId
                };

                _context.Educations.Add(education);
                await _context.SaveChangesAsync();

                // WhatYouWillLearn kayıtlarını ekle
                if (model.WhatYouWillLearnFields != null && model.WhatYouWillLearnFields.Any())
                {
                    var whatYouWillLearns = model.WhatYouWillLearnFields
                        .Where(f => !string.IsNullOrWhiteSpace(f))
                        .Select(f => new WhatYouWillLearn
                        {
                            Title = f.Trim(),
                            EducationId = education.Id
                        }).ToList();

                    _context.WhatYouWillLearns.AddRange(whatYouWillLearns);
                }

                // Requirements kayıtlarını ekle
                if (model.RequirementFields != null && model.RequirementFields.Any())
                {
                    var requirements = model.RequirementFields
                        .Where(f => !string.IsNullOrWhiteSpace(f))
                        .Select(f => new Requirement
                        {
                            Title = f.Trim(),
                            EducationId = education.Id
                        }).ToList();

                    _context.Requirements.AddRange(requirements);
                }

                // CourseContents kayıtlarını ekle
                if (model.CourseContentFields != null && model.CourseContentFields.Any())
                {
                    var courseContents = model.CourseContentFields
                        .Where(f => !string.IsNullOrWhiteSpace(f))
                        .Select(f => new CourseContent
                        {
                            Title = f.Trim(),
                            EducationId = education.Id
                        }).ToList();

                    _context.CourseContents.AddRange(courseContents);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                TempData["SuccessMessage"] = "Eğitim başarıyla eklendi!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError("", "Kayıt sırasında bir hata oluştu: " + ex.Message);
            }

            await LoadSelectLists();
            return View(model);
        }

        // GET: AdminEducation/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var education = await _context.Educations
                .Include(e => e.WhatYouWillLearns)
                .Include(e => e.Requirements)
                .Include(e => e.CourseContents)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (education == null)
            {
                return NotFound();
            }

            var viewModel = new EducationEditViewModel
            {
                Id = education.Id,
                Name = education.Name,
                ShortDescription = education.ShortDescription,
                LongDescription = education.Description,
                StudentCount = education.StudentCount,
                CourseHours = education.CourseHours,
                Language = education.Language,
                PhotoUrl = education.PhotoUrl,
                IsPopularCourse = education.IsPopularCourse,
                IsActive = education.IsActive,
                InstructorId = education.InstructorId,
                FeatureId = education.FeatureId,

                // Mevcut kayıtları al
                ExistingWhatYouWillLearn = education.WhatYouWillLearns.Select(w => w.Title).ToList(),
                ExistingRequirements = education.Requirements.Select(r => r.Title).ToList(),
                ExistingCourseContents = education.CourseContents.Select(c => c.Title).ToList()
            };

            await LoadSelectLists();
            ViewBag.EducationId = id;
            ViewBag.ExistingWhatYouWillLearnNames = string.Join(", ", viewModel.ExistingWhatYouWillLearn);
            ViewBag.ExistingRequirementsNames = string.Join(", ", viewModel.ExistingRequirements);
            ViewBag.ExistingCourseContentsNames = string.Join(", ", viewModel.ExistingCourseContents);

            return View(viewModel);
        }

        // POST: AdminEducation/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EducationEditViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var education = await _context.Educations
                        .Include(e => e.WhatYouWillLearns)
                        .Include(e => e.Requirements)
                        .Include(e => e.CourseContents)
                        .FirstOrDefaultAsync(e => e.Id == id);

                    if (education == null)
                    {
                        return NotFound();
                    }

                    // Fotoğraf güncelleme işlemi
                    string photoFileName = education.PhotoUrl; // Mevcut fotoğrafı koru
                    if (model.PhotoFile != null && model.PhotoFile.Length > 0)
                    {
                        // Eski fotoğrafı sil
                        if (!string.IsNullOrEmpty(education.PhotoUrl))
                        {
                            await DeletePhotoAsync(education.PhotoUrl);
                        }
                        // Yeni fotoğrafı kaydet
                        photoFileName = await SavePhotoAsync(model.PhotoFile);
                    }

                    // Education temel bilgilerini güncelle
                    education.Name = model.Name;
                    education.ShortDescription = model.ShortDescription;
                    education.Description = model.LongDescription ?? "";
                    education.StudentCount = model.StudentCount;
                    education.CourseHours = model.CourseHours;
                    education.Language = model.Language;
                    education.PhotoUrl = photoFileName;
                    education.IsPopularCourse = model.IsPopularCourse;
                    education.IsActive = model.IsActive;
                    education.InstructorId = model.InstructorId;
                    education.FeatureId = model.FeatureId;

                    // Mevcut bağlantılı kayıtları sil
                    _context.WhatYouWillLearns.RemoveRange(education.WhatYouWillLearns);
                    _context.Requirements.RemoveRange(education.Requirements);
                    _context.CourseContents.RemoveRange(education.CourseContents);

                    // Yeni kayıtları ekle
                    if (model.WhatYouWillLearnFields != null && model.WhatYouWillLearnFields.Any())
                    {
                        var whatYouWillLearns = model.WhatYouWillLearnFields
                            .Where(f => !string.IsNullOrWhiteSpace(f))
                            .Select(f => new WhatYouWillLearn
                            {
                                Title = f.Trim(),
                                EducationId = education.Id
                            }).ToList();

                        _context.WhatYouWillLearns.AddRange(whatYouWillLearns);
                    }

                    if (model.RequirementFields != null && model.RequirementFields.Any())
                    {
                        var requirements = model.RequirementFields
                            .Where(f => !string.IsNullOrWhiteSpace(f))
                            .Select(f => new Requirement
                            {
                                Title = f.Trim(),
                                EducationId = education.Id
                            }).ToList();

                        _context.Requirements.AddRange(requirements);
                    }

                    if (model.CourseContentFields != null && model.CourseContentFields.Any())
                    {
                        var courseContents = model.CourseContentFields
                            .Where(f => !string.IsNullOrWhiteSpace(f))
                            .Select(f => new CourseContent
                            {
                                Title = f.Trim(),
                                EducationId = education.Id
                            }).ToList();

                        _context.CourseContents.AddRange(courseContents);
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    TempData["SuccessMessage"] = "Eğitim başarıyla güncellendi!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    await transaction.RollbackAsync();
                    if (!EducationExists(model.Id))
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
                    await transaction.RollbackAsync();
                    ModelState.AddModelError("", "Güncelleme sırasında bir hata oluştu: " + ex.Message);
                }
            }

            await LoadSelectLists();
            ViewBag.EducationId = id;
            return View(model);
        }

        // GET: AdminEducation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var education = await _context.Educations
                .Include(e => e.Instructor)
                .Include(e => e.Feature)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (education == null)
            {
                return NotFound();
            }

            return View(education);
        }

        // POST: AdminEducation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var education = await _context.Educations.FindAsync(id);
            if (education != null)
            {
                // Fotoğrafı sil
                if (!string.IsNullOrEmpty(education.PhotoUrl))
                {
                    await DeletePhotoAsync(education.PhotoUrl);
                }

                _context.Educations.Remove(education);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Eğitim başarıyla silindi!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool EducationExists(int id)
        {
            return _context.Educations.Any(e => e.Id == id);
        }

        private async Task LoadSelectLists()
        {
            try
            {
                // Instructors listesi
                var instructors = await _context.Instructors
                    .Where(i => i.IsActive)
                    .ToListAsync();

                if (instructors != null && instructors.Any())
                {
                    ViewBag.InstructorId = new SelectList(instructors, "InstructorId", "NameSurname");
                }
                else
                {
                    ViewBag.InstructorId = new SelectList(new List<SelectListItem>(), "Value", "Text");
                }

                // Features listesi
                var features = await _context.Features.ToListAsync();
                if (features != null && features.Any())
                {
                    ViewBag.FeatureId = new SelectList(features, "Id", "Name");
                }
                else
                {
                    ViewBag.FeatureId = new SelectList(new List<SelectListItem>(), "Value", "Text");
                }
            }
            catch (Exception ex)
            {
                ViewBag.InstructorId = new SelectList(new List<SelectListItem>(), "Value", "Text");
                ViewBag.FeatureId = new SelectList(new List<SelectListItem>(), "Value", "Text");
                Console.WriteLine($"LoadSelectLists Error: {ex.Message}");
            }
        }

        // Fotoğraf kaydetme işlemi
        private async Task<string> SavePhotoAsync(IFormFile photoFile)
        {
            try
            {
                // Dosya uzantısını kontrol et
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(photoFile.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    throw new InvalidOperationException("Sadece jpg, jpeg, png ve gif formatları desteklenir.");
                }

                // Dosya boyutunu kontrol et (5MB)
                if (photoFile.Length > 5 * 1024 * 1024)
                {
                    throw new InvalidOperationException("Dosya boyutu 5MB'dan büyük olamaz.");
                }

                // Unique dosya adı oluştur
                var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;

                // Upload klasörünü oluştur
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "educations");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Dosya yolunu oluştur
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Dosyayı kaydet
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await photoFile.CopyToAsync(fileStream);
                }

                // Veritabanında saklanacak relatif yol
                return $"/uploads/educations/{uniqueFileName}";
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Fotoğraf kaydedilirken hata oluştu: {ex.Message}");
            }
        }

        // Fotoğraf silme işlemi
        private async Task DeletePhotoAsync(string photoUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(photoUrl))
                    return;

                // Fiziksel dosya yolunu oluştur
                var fileName = Path.GetFileName(photoUrl);
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "educations", fileName);

                // Dosya varsa sil
                if (System.IO.File.Exists(filePath))
                {
                    await Task.Run(() => System.IO.File.Delete(filePath));
                }
            }
            catch (Exception ex)
            {
                // Log the error but don't throw - deletion of old photo shouldn't prevent the update
                Console.WriteLine($"Fotoğraf silinirken hata oluştu: {ex.Message}");
            }
        }
    }
}