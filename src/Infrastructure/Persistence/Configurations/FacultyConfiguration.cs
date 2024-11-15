using Domain.Faculties;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class FacultyConfiguration : IEntityTypeConfiguration<Faculty>
{
    public void Configure(EntityTypeBuilder<Faculty> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => new FacultyId(x));

        builder.Property(x => x.Name).IsRequired().HasColumnType("varchar(255)");

        builder.OwnsOne(x => x.Equipment, equipmentBuilder =>
        {
            equipmentBuilder.ToJson("equipment");

            equipmentBuilder.Property(x => x.Type).HasJsonPropertyName("type");

            equipmentBuilder.OwnsOne(x => x.MedicalEquipment, medicalBuilder =>
            {
                medicalBuilder.Property(x => x.Name).HasJsonPropertyName("name");
                medicalBuilder.Property(x => x.Price).HasJsonPropertyName("price");
            });

            equipmentBuilder.OwnsOne(x => x.TechnicalEquipment, technicalEquipment =>
            {
                technicalEquipment.Property(x => x.Name).HasJsonPropertyName("name");
                technicalEquipment.Property(x => x.Weight).HasJsonPropertyName("weight");

                technicalEquipment.OwnsMany(x => x.Parts, partsBuilder =>
                {
                    partsBuilder.Property(x => x.Name).HasJsonPropertyName("name");
                });
            });
        });
    }
}