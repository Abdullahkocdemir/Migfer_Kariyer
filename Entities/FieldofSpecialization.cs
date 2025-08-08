using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Migfer_Kariyer.Entities
{
    public class FieldofSpecialization
    {
        public int FieldofSpecializationId { get; set; }
        public string Name { get; set; } = string.Empty;

        // Eğitmenlerle ilişki
        public ICollection<InstructorFieldofSpecialization> InstructorFieldofSpecializations { get; set; } = new List<InstructorFieldofSpecialization>();
    }

}
