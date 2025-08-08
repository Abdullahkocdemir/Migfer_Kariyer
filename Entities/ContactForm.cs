using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Migfer_Kariyer.Entities
{
    public class ContactForm
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "VarChar")]
        [StringLength(30)]
        public string Name { get; set; } = string.Empty;
        [Column(TypeName = "VarChar")]
        [StringLength(30)]
        public string SurName { get; set; } = string.Empty;
        [Column(TypeName = "VarChar")]
        [StringLength(30)]
        public string Email { get; set; } = string.Empty;
        [Column(TypeName = "VarChar")]
        [StringLength(30)]
        public string PhoneNumber { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        // Foreign Key
        public int? SubjectId { get; set; }

        // Navigation Property
        public Subject? Subject { get; set; }
    }
}
