using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Migfer_Kariyer.Entities
{
    public class Instructor
    {
        public int InstructorId { get; set; }

        [Column(TypeName = "VarChar")]
        [StringLength(30)]
        public string NameSurname { get; set; } = string.Empty;

        [Column(TypeName = "VarChar")]
        [StringLength(30)]
        public string StudentCount { get; set; } = string.Empty;

        [Column(TypeName = "VarChar")]
        [StringLength(30)]
        public string Experience { get; set; } = string.Empty;

        [Column(TypeName = "VarChar")]
        [StringLength(30)]
        public string PhoneNumber { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true; // Eğitmenin aktif olup olmadığını belirten alan
        public string Email { get; set; } = string.Empty;
        // Bu eğitmenin verdiği eğitimler
        public ICollection<Education> Educations { get; set; } = new List<Education>();
        // Uzmanlık alanlarıyla ilişki
        public ICollection<InstructorFieldofSpecialization> InstructorFieldofSpecializations { get; set; } = new List<InstructorFieldofSpecialization>();
    }

}
