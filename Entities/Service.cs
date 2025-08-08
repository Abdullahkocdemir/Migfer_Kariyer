
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Migfer_Kariyer.Entities
{
    public class Service
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Başlık { get; set; } = string.Empty;

        [Required]
        [MaxLength(1000)]
        public string Açıklama { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string İkon { get; set; } = string.Empty;

        // Navigation Property - Bir hizmete birden fazla madde
        public virtual ICollection<Maddeler> Maddelers { get; set; } = new List<Maddeler>();
    }
}
