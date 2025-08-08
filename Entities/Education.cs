using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Migfer_Kariyer.Entities
{
    public class Education
    {
        public int Id { get; set; }

        [Column(TypeName = "VarChar")]
        [StringLength(30)]
        public string Name { get; set; } = string.Empty;
        
        [Column(TypeName = "VarChar")]
        [StringLength(300)]
        public string ShortDescription { get; set; } = string.Empty;
        public int StudentCount { get; set; }
        public int CourseHours { get; set; }

        [Column(TypeName = "VarChar")]
        [StringLength(30)]
        public string Language { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
        public bool IsPopularCourse { get; set; }
        public bool IsActive { get; set; }

        // Bir eğitime bir eğitmen
        public int InstructorId { get; set; }
        public Instructor? Instructor { get; set; }

        // Bir eğitime bir özellik
        public int FeatureId { get; set; }
        public Feature? Feature { get; set; }

        // Bir eğitime birden fazla "neler öğreneceksiniz"
        public ICollection<WhatYouWillLearn> WhatYouWillLearns { get; set; } = new List<WhatYouWillLearn>();

        // Bir eğitime birden fazla gereksinim
        public ICollection<Requirement> Requirements { get; set; } = new List<Requirement>();

        // Bir eğitime birden fazla kurs içeriği
        public ICollection<CourseContent> CourseContents { get; set; } = new List<CourseContent>();
    }
}
