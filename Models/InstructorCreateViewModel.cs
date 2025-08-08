using Migfer_Kariyer.Entities;
using System.ComponentModel.DataAnnotations;

namespace Migfer_Kariyer.Models
{
    public class InstructorCreateViewModel
    {
        [Required(ErrorMessage = "Ad Soyad alanı zorunludur")]
        [StringLength(100, ErrorMessage = "Ad Soyad en fazla 100 karakter olabilir")]
        public string NameSurname { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "Öğrenci sayısı en fazla 50 karakter olabilir")]
        public string StudentCount { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Deneyim en fazla 100 karakter olabilir")]
        public string Experience { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true; // Eğitmenin aktif olup olmadığını belirten alan

        // Dinamik olarak eklenen yeni uzmanlık alanları (tek tek)
        public List<string> DynamicFields { get; set; } = new();


        // Mevcut uzmanlık alanları (artık kullanılmıyor ama uyumluluk için bırakıldı)
        public List<int> SelectedFieldIds { get; set; } = new();
        public List<FieldofSpecialization> AvailableFields { get; set; } = new();
    }
}