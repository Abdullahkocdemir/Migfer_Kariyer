using System.ComponentModel.DataAnnotations;

namespace Migfer_Kariyer.Models
{
    // Create için ViewModel
    public class EducationCreateViewModel
    {
        [Required(ErrorMessage = "Eğitim adı zorunludur")]
        [StringLength(30, ErrorMessage = "Eğitim adı en fazla 30 karakter olabilir")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kısa açıklama zorunludur")]
        [StringLength(300, ErrorMessage = "Kısa açıklama en fazla 300 karakter olabilir")]
        public string ShortDescription { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Uzun açıklama en fazla 1000 karakter olabilir")]
        public string? LongDescription { get; set; }

        [Required(ErrorMessage = "Öğrenci sayısı zorunludur")]
        [Range(1, 1000, ErrorMessage = "Öğrenci sayısı 1-1000 arasında olmalıdır")]
        public int StudentCount { get; set; }

        [Required(ErrorMessage = "Kurs saati zorunludur")]
        [Range(1, 500, ErrorMessage = "Kurs saati 1-500 arasında olmalıdır")]
        public int CourseHours { get; set; }

        [Required(ErrorMessage = "Dil zorunludur")]
        [StringLength(30, ErrorMessage = "Dil en fazla 30 karakter olabilir")]
        public string Language { get; set; } = string.Empty;

        // Fotoğraf yükleme için
        public IFormFile? PhotoFile { get; set; }

        public bool IsPopularCourse { get; set; } = false;
        public bool IsActive { get; set; } = true;

        [Required(ErrorMessage = "Eğitmen seçimi zorunludur")]
        public int InstructorId { get; set; }

        [Required(ErrorMessage = "Özellik seçimi zorunludur")]
        public int FeatureId { get; set; }

        // Dinamik alanlar
        public List<string> WhatYouWillLearnFields { get; set; } = new List<string>();
        public List<string> RequirementFields { get; set; } = new List<string>();
        public List<string> CourseContentFields { get; set; } = new List<string>();
    }

    // Edit için ViewModel
    public class EducationEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Eğitim adı zorunludur")]
        [StringLength(30, ErrorMessage = "Eğitim adı en fazla 30 karakter olabilir")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kısa açıklama zorunludur")]
        [StringLength(300, ErrorMessage = "Kısa açıklama en fazla 300 karakter olabilir")]
        public string ShortDescription { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Uzun açıklama en fazla 1000 karakter olabilir")]
        public string? LongDescription { get; set; }

        [Required(ErrorMessage = "Öğrenci sayısı zorunludur")]
        [Range(1, 1000, ErrorMessage = "Öğrenci sayısı 1-1000 arasında olmalıdır")]
        public int StudentCount { get; set; }

        [Required(ErrorMessage = "Kurs saati zorunludur")]
        [Range(1, 500, ErrorMessage = "Kurs saati 1-500 arasında olmalıdır")]
        public int CourseHours { get; set; }

        [Required(ErrorMessage = "Dil zorunludur")]
        [StringLength(30, ErrorMessage = "Dil en fazla 30 karakter olabilir")]
        public string Language { get; set; } = string.Empty;

        // Mevcut fotoğraf URL'i
        public string PhotoUrl { get; set; } = string.Empty;

        // Yeni fotoğraf yükleme için
        public IFormFile? PhotoFile { get; set; }

        public bool IsPopularCourse { get; set; } = false;
        public bool IsActive { get; set; } = true;

        [Required(ErrorMessage = "Eğitmen seçimi zorunludur")]
        public int InstructorId { get; set; }

        [Required(ErrorMessage = "Özellik seçimi zorunludur")]
        public int FeatureId { get; set; }

        // Dinamik alanlar
        public List<string> WhatYouWillLearnFields { get; set; } = new List<string>();
        public List<string> RequirementFields { get; set; } = new List<string>();
        public List<string> CourseContentFields { get; set; } = new List<string>();

        // Mevcut kayıtları göstermek için
        public List<string> ExistingWhatYouWillLearn { get; set; } = new List<string>();
        public List<string> ExistingRequirements { get; set; } = new List<string>();
        public List<string> ExistingCourseContents { get; set; } = new List<string>();
    }
}