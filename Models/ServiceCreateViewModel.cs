using System.ComponentModel.DataAnnotations;

namespace Migfer_Kariyer.Models
{
    public class ServiceCreateViewModel
    {
        [Required(ErrorMessage = "Başlık alanı gereklidir.")]
        [MaxLength(200, ErrorMessage = "Başlık en fazla 200 karakter olabilir.")]
        public string Başlık { get; set; } = string.Empty;

        [Required(ErrorMessage = "Açıklama alanı gereklidir.")]
        [MaxLength(1000, ErrorMessage = "Açıklama en fazla 1000 karakter olabilir.")]
        public string Açıklama { get; set; } = string.Empty;

        [Required(ErrorMessage = "İkon alanı gereklidir.")]
        [MaxLength(200, ErrorMessage = "İkon en fazla 200 karakter olabilir.")]
        public string İkon { get; set; } = string.Empty;

        // Dinamik alanlar için
        public List<string> MaddelerFields { get; set; } = new List<string>();
    }

    public class ServiceEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlık alanı gereklidir.")]
        [MaxLength(200, ErrorMessage = "Başlık en fazla 200 karakter olabilir.")]
        public string Başlık { get; set; } = string.Empty;

        [Required(ErrorMessage = "Açıklama alanı gereklidir.")]
        [MaxLength(1000, ErrorMessage = "Açıklama en fazla 1000 karakter olabilir.")]
        public string Açıklama { get; set; } = string.Empty;

        [Required(ErrorMessage = "İkon alanı gereklidir.")]
        [MaxLength(200, ErrorMessage = "İkon en fazla 200 karakter olabilir.")]
        public string İkon { get; set; } = string.Empty;

        // Dinamik alanlar için
        public List<string> MaddelerFields { get; set; } = new List<string>();

        // Mevcut kayıtları göstermek için
        public List<string> ExistingMaddelers { get; set; } = new List<string>();
    }
}