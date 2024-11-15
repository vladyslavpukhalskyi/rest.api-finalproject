using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Movies.Commands;
using Domain.Movies;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("movies")]
[ApiController]
public class MoviesController(ISender sender, IMovieQueries movieQueries) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<MovieDto>>> GetAll(CancellationToken cancellationToken)
    {
        var entities = await movieQueries.GetAll(cancellationToken);

        return entities.Select(MovieDto.FromDomainModel).ToList();
    }

    [HttpGet("{movieId:guid}")]
    public async Task<ActionResult<MovieDto>> Get([FromRoute] Guid movieId, CancellationToken cancellationToken)
    {
        var entity = await movieQueries.GetById(new MovieId(movieId), cancellationToken);

        return entity.Match<ActionResult<MovieDto>>(
            m => MovieDto.FromDomainModel(m),
            () => NotFound());
    }

    [HttpPost]
    public async Task<ActionResult<MovieDto>> Create([FromBody] MovieDto request, CancellationToken cancellationToken)
    {
        var input = new CreateMovieCommand
        {
            Title = request.Title,
            ReleaseYear = request.ReleaseYear,
            GenreId = request.GenreId,
            DirectorId = request.DirectorId,
            ActorIds = request.ActorIds,
            Director = null,
            ReleaseDate = default
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<MovieDto>>(
            m => MovieDto.FromDomainModel(m),
            e => e.ToObjectResult());
    }

    [HttpPut]
    public async Task<ActionResult<MovieDto>> Update([FromBody] MovieDto request, CancellationToken cancellationToken)
    {
        var input = new UpdateMovieCommand
        {
            MovieId = request.Id!.Value,
            Title = request.Title,
            ReleaseYear = request.ReleaseYear,
            GenreId = request.GenreId,
            DirectorId = request.DirectorId,
            ActorIds = request.ActorIds,
            Genre = null,
            ReleaseDate = default
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<MovieDto>>(
            movie => MovieDto.FromDomainModel(movie),
            e => e.ToObjectResult());
    }

    [HttpDelete("{movieId:guid}")]
    public async Task<ActionResult<MovieDto>> Delete([FromRoute] Guid movieId, CancellationToken cancellationToken)
    {
        var input = new DeleteMovieCommand
        {
            MovieId = movieId
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<MovieDto>>(
            m => MovieDto.FromDomainModel(m),
            e => e.ToObjectResult());
    }
}
