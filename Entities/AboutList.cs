using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Migfer_Kariyer.Entities
{
    public class AboutList
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "NVARCHAR")] // Unicode için NVARCHAR
        [StringLength(30)]
        public string Başlık { get; set; } = string.Empty;

        [Column(TypeName = "NVARCHAR")] // Unicode için NVARCHAR
        [StringLength(2000)]
        public string Açıklama { get; set; } = string.Empty;

        [Column(TypeName = "NVARCHAR")] // Unicode için NVARCHAR - Emoji desteği
        [StringLength(30)]
        public string Ikon { get; set; } = string.Empty;
    }
}