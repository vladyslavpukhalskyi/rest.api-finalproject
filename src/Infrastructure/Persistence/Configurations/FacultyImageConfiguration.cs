using Domain.Faculties;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/*public class FacultyImageConfiguration : IEntityTypeConfiguration<FacultyImage>
{
    public void Configure(EntityTypeBuilder<FacultyImage> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => new FacultyImageId(x));

        builder.Property(x => x.FacultyId).HasConversion(x => x.Value, x => new FacultyId(x));
        builder.HasOne(x => x.Faculty)
            .WithMany()
            .HasForeignKey(x => x.FacultyId)
            .HasConstraintName("fk_faculty_images_faculties_id")
            .OnDelete(DeleteBehavior.Cascade);
    }
}*/