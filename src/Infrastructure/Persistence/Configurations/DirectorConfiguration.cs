using Domain.Directors;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class DirectorConfiguration : IEntityTypeConfiguration<Director>
{
    public void Configure(EntityTypeBuilder<Director> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => new DirectorId(x));

        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnType("varchar(255)");

        builder.Property(x => x.BirthDate)
            .IsRequired()
            .HasColumnType("date");

        // Зв'язок з фільмами (один до багатьох)
        builder.HasMany(x => x.Movies)
            .WithOne(x => x.Director)
            .HasForeignKey(x => x.DirectorId)
            .HasConstraintName("fk_movies_directors_id")
            .OnDelete(DeleteBehavior.Restrict);
    }
}