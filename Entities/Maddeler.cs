using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Migfer_Kariyer.Entities
{
    public class Maddeler
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string Name { get; set; } = string.Empty;

        // Foreign Key
        [Required]
        public int ServicesId { get; set; }

        // Navigation Property
        [ForeignKey("ServicesId")]
        public virtual Service? Services { get; set; }
    }
}