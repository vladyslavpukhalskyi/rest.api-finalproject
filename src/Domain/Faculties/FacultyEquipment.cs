namespace Domain.Faculties;

public class FacultyEquipment
{
    public string Type { get; }

    public MedicalEquipment? MedicalEquipment { get; private init; }
    public TechnicalEquipment? TechnicalEquipment { get; private init; }

    private FacultyEquipment(string type)
    {
        Type = type;
    }

    public static FacultyEquipment NewMedicalEquipment(MedicalEquipment medicalEquipment)
        => new("medical") { MedicalEquipment = medicalEquipment };

    public static FacultyEquipment NewTechnicalEquipment(TechnicalEquipment technicalEquipment)
        => new("technical") { TechnicalEquipment = technicalEquipment };
}

public record MedicalEquipment
{
    public required string Name { get; init; }
    public required decimal Price { get; init; }
}

public record TechnicalEquipment
{
    public required string Name { get; init; }
    public required double Weight { get; init; }

    public required List<TechnicalEquipmentPart> Parts { get; init; }
}

public record TechnicalEquipmentPart
{
    public required string Name { get; init; }
}