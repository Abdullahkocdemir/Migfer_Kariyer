namespace Migfer_Kariyer.Entities
{
    public class InstructorFieldofSpecialization
    {
        public int InstructorId { get; set; }
        public Instructor? Instructor { get; set; }

        public int FieldofSpecializationId { get; set; }
        public FieldofSpecialization? FieldofSpecialization { get; set; }
    }
}
