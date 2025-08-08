using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Migfer_Kariyer.Entities
{
    public class AboutText
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "VarChar")]
        [StringLength(30)]
        public string Başlık { get; set; } = string.Empty;

        [Column(TypeName = "VarChar")]
        [StringLength(30)]
        public string Açıklama { get; set; } = string.Empty;
        public bool IsActive { get; set; } = false;
    }
}
