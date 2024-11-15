using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Movies.Exceptions;
using Domain.Movies;
using MediatR;

namespace Application.Movies.Commands;

public record CreateMovieCommand : IRequest<Result<Movie, MovieException>>
{
    public required string Title { get; init; }
    public required string Director { get; init; }
    public required Guid GenreId { get; init; }
    public required DateTime ReleaseDate { get; init; }
}

public class CreateMovieCommandHandler(
    IMovieRepository movieRepository,
    IGenreRepository genreRepository)
    : IRequestHandler<CreateMovieCommand, Result<Movie, MovieException>>
{
    public async Task<Result<Movie, MovieException>> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
    {
        var genreId = new GenreId(request.GenreId);

        var genre = await genreRepository.GetById(genreId, cancellationToken);

        return await genre.Match<Task<Result<Movie, MovieException>>>( 
            async g =>
            {
                var existingMovie = await movieRepository.GetByTitle(request.Title, cancellationToken);

                return await existingMovie.Match(
                    m => Task.FromResult<Result<Movie, MovieException>>(new MovieAlreadyExistsException(m.Id)),
                    async () => await CreateEntity(request.Title, request.Director, g.Id, request.ReleaseDate, cancellationToken));
            },
            () => Task.FromResult<Result<Movie, MovieException>>(new MovieGenreNotFoundException(genreId)));
    }

    private async Task<Result<Movie, MovieException>> CreateEntity(
        string title,
        string director,
        GenreId genreId,
        DateTime releaseDate,
        CancellationToken cancellationToken)
    {
        try
        {
            var entity = Movie.New(MovieId.New(), title, director, genreId, releaseDate);

            return await movieRepository.Add(entity, cancellationToken);
        }
        catch (Exception exception)
        {
            return new MovieUnknownException(MovieId.Empty(), exception);
        }
    }
}
