using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Migfer_Kariyer.Entities
{
    public class ContactList
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "NVARCHAR")] // VARCHAR yerine NVARCHAR
        [StringLength(30)]
        public string Adı { get; set; } = string.Empty;

        [Column(TypeName = "NVARCHAR")] // VARCHAR yerine NVARCHAR
        [StringLength(500)]
        public string Açıklama { get; set; } = string.Empty;

        [Column(TypeName = "NVARCHAR")] // VARCHAR yerine NVARCHAR - Unicode için!
        [StringLength(30)]
        public string İkon { get; set; } = string.Empty;
    }
}