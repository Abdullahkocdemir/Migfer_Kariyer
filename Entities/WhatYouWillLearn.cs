using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Migfer_Kariyer.Entities
{
    // Neler Öğreneceksiniz Entity'si
    public class WhatYouWillLearn
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
