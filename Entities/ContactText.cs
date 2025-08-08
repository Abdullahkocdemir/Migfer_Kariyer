using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Migfer_Kariyer.Entities
{
    public class ContactText
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "VarChar")]
        [StringLength(30)]
        public string Adres { get; set; } = string.Empty;

        [Column(TypeName = "VarChar")]
        [StringLength(30)]
        public string Telefon { get; set; } = string.Empty;

        [Column(TypeName = "VarChar")]
        [StringLength(30)]
        public string Eposta { get; set; } = string.Empty;

        [Column(TypeName = "VarChar")]
        [StringLength(30)]
        public string WhatsApp { get; set; } = string.Empty;
    }
}
