using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Migfer_Kariyer.Entities
{
    // Özellik Entity'si
    public class Feature
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "NVARCHAR")] // Unicode için NVARCHAR
        [StringLength(30)]
        public string Name { get; set; } = string.Empty;

        // Bu özelliğe sahip eğitimler - Navigation Property
        public virtual ICollection<Education> Educations { get; set; } = new List<Education>();
    }
}