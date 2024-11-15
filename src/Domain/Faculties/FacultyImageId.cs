namespace Domain.Faculties;

public record FacultyImageId(Guid Value)
{
    public static FacultyImageId Empty => new(Guid.Empty);
    public static FacultyImageId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}