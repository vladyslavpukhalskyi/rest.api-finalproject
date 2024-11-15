namespace Application.Genres.Exceptions;

public abstract class GenreException(Guid genreId, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public Guid GenreId { get; } = genreId;
}

public class GenreNotFoundException(Guid genreId)
    : GenreException(genreId, $"Genre under id: {genreId} not found");

public class GenreAlreadyExistsException(Guid genreId)
    : GenreException(genreId, $"Genre already exists: {genreId}");

public class GenreUnknownException(Guid genreId, Exception innerException)
    : GenreException(genreId, $"Unknown exception for the genre under id: {genreId}", innerException);