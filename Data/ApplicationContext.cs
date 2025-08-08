using Microsoft.EntityFrameworkCore;
using Migfer_Kariyer.Entities;

namespace Migfer_Kariyer.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        public DbSet<Education> Educations { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<AboutText> AboutTexts { get; set; }
        public DbSet<AboutList> AboutLists { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<ContactText> ContactTexts { get; set; }
        public DbSet<ContactList> ContactLists { get; set; }
        public DbSet<ContactForm> ContactForms { get; set; }
        public DbSet<Maddeler> Maddelers { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<SocialMedia> SocialMedias { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<FieldofSpecialization> FieldofSpecializations { get; set; }
        public DbSet<WhatYouWillLearn> WhatYouWillLearns { get; set; }
        public DbSet<Requirement> Requirements { get; set; }
        public DbSet<CourseContent> CourseContents { get; set; }
        public DbSet<InstructorFieldofSpecialization> InstructorFieldofSpecializations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Instructor - FieldofSpecialization Many-to-Many
            modelBuilder.Entity<InstructorFieldofSpecialization>()
                .HasKey(x => new { x.InstructorId, x.FieldofSpecializationId });

            modelBuilder.Entity<InstructorFieldofSpecialization>()
                .HasOne(x => x.Instructor)
                .WithMany(i => i.InstructorFieldofSpecializations)
                .HasForeignKey(x => x.InstructorId);

            modelBuilder.Entity<InstructorFieldofSpecialization>()
                .HasOne(x => x.FieldofSpecialization)
                .WithMany(f => f.InstructorFieldofSpecializations)
                .HasForeignKey(x => x.FieldofSpecializationId);

            // Education - Instructor (One-to-Many)
            modelBuilder.Entity<Education>()
                .HasOne(e => e.Instructor)
                .WithMany(i => i.Educations)
                .HasForeignKey(e => e.InstructorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Education - Feature (One-to-Many)
            modelBuilder.Entity<Education>()
                .HasOne(e => e.Feature)
                .WithMany(f => f.Educations)
                .HasForeignKey(e => e.FeatureId)
                .OnDelete(DeleteBehavior.Restrict);

            // Education - WhatYouWillLearn (One-to-Many)
            modelBuilder.Entity<WhatYouWillLearn>()
                .HasOne(w => w.Education)
                .WithMany(e => e.WhatYouWillLearns)
                .HasForeignKey(w => w.EducationId)
                .OnDelete(DeleteBehavior.Cascade);

            // Education - Requirement (One-to-Many)
            modelBuilder.Entity<Requirement>()
                .HasOne(r => r.Education)
                .WithMany(e => e.Requirements)
                .HasForeignKey(r => r.EducationId)
                .OnDelete(DeleteBehavior.Cascade);

            // Education - CourseContent (One-to-Many)
            modelBuilder.Entity<CourseContent>()
                .HasOne(c => c.Education)
                .WithMany(e => e.CourseContents)
                .HasForeignKey(c => c.EducationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}