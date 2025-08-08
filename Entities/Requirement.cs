using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Migfer_Kariyer.Entities
{
    // Gereksinimler Entity'si
    public class Requirement
    {
        public int Id { get; set; }

        [Column(TypeName = "VarChar")]
        [StringLength(30)]
        public string Title { get; set; } = string.Empty;

        // Hangi eğitime ait
        public int EducationId { get; set; }
        public Education? Education { get; set; }
    }
}
