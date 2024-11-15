namespace Domain.Faculties;

public class FacultyImage
{
    public FacultyImageId Id { get; }

    public FacultyId FacultyId { get; }
    public Faculty? Faculty { get; }

    private FacultyImage(FacultyImageId id, FacultyId facultyId)
    {
        Id = id;
        FacultyId = facultyId;
    }

    public static FacultyImage New(FacultyImageId id, FacultyId facultyId) => new(id, facultyId);

    public string FilePath => $"{FacultyId}/{Id}.jpeg";
}