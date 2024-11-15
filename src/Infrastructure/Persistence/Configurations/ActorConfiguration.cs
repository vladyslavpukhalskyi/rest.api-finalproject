using Domain.Actors;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ActorConfiguration : IEntityTypeConfiguration<Actor>
{
    public void Configure(EntityTypeBuilder<Actor> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => new ActorId(x));

        builder.Property(x => x.Name).IsRequired().HasColumnType("varchar(255)");

        // Зв'язок з фільмами "багато до багатьох"
        builder.HasMany(x => x.Movies)
            .WithMany(x => x.Actors)
            .UsingEntity(j => j.ToTable("MovieActors"));

        // Додаткові властивості актора, наприклад, дата народження
        builder.Property(x => x.DateOfBirth)
            .HasColumnType("date");

        builder.Property(x => x.Nationality)
            .HasColumnType("varchar(100)");
    }
}