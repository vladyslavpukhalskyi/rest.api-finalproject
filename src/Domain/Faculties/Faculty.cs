namespace Domain.Faculties;

public class Faculty
{
    public FacultyId Id { get; }

    public string Name { get; private set; }

    public FacultyEquipment Equipment { get; private init; }

    // public ICollection<FacultyImage>? Images { get; }
    //
    // public string LogoImagePath => $"faculty-images/{Id}.jpeg";

    private Faculty(FacultyId id, string name)
    {
        Id = id;
        Name = name;
    }

    public static Faculty New(FacultyId id, string name, FacultyEquipment equipment)
        => new(id, name) { Equipment = equipment };

    public void UpdateDetails(string name)
    {
        Name = name;
    }
}