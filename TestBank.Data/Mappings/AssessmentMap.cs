using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestBank.Entity;
using System.Data.Entity.ModelConfiguration;

namespace TestBank.Data.Mappings
{
    public class AssessmentMap : EntityTypeConfiguration<Assessment>
    {
        public AssessmentMap()
        {
            this.ToTable("Assessments");

            /*
            // Primary Key
            this.HasKey(x => x.Id);

            // Properties
            this.Property(x => x.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(x => x.Measure)
                .IsRequired()
                .HasMaxLength(30);

            // Table & Column Mappings
            this.ToTable("Products");
            this.Property(x => x.Id).HasColumnName("Id");
            this.Property(x => x.Name).HasColumnName("Name");
            this.Property(x => x.Description).HasColumnName("Description");
            this.Property(x => x.Measure).HasColumnName("Measure");
            this.Property(x => x.Price).HasColumnName("Price");
            this.Property(x => x.ScheduleId).HasColumnName("ScheduleId");
            this.Property(x => x.CategoryId).HasColumnName("CategoryId");

            // Relationships
            this.HasRequired(x => x.Category)
                .WithMany(x => x.Products)
                .HasForeignKey(d => d.CategoryId);
            this.HasRequired(x => x.Schedule)
                .WithMany(x => x.Products)
                .HasForeignKey(d => d.ScheduleId);

            this.Ignore(x => x.ObjectState);*/
        }
    }
}
