using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Actors;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class ActorRepository(ApplicationDbContext context) : IActorRepository, IActorQueries
{
    public async Task<Option<Actor>> GetById(ActorId id, CancellationToken cancellationToken)
    {
        var entity = await context.Actors
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity == null ? Option.None<Actor>() : Option.Some(entity);
    }

    public Task<Option<Actor>> GetByFullName(string firstName, string lastName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Option<Actor>> GetByName(string name, CancellationToken cancellationToken)
    {
        var entity = await context.Actors
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == name, cancellationToken);

        return entity == null ? Option.None<Actor>() : Option.Some(entity);
    }

    public async Task<IReadOnlyList<Actor>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Actors
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Actor>> GetByMovieId(MovieId movieId, CancellationToken cancellationToken)
    {
        return await context.Actors
            .Where(x => x.Movies.Any(m => m.Id == movieId))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Actor> Add(Actor actor, CancellationToken cancellationToken)
    {
        await context.Actors.AddAsync(actor, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return actor;
    }

    public async Task<Actor> Update(Actor actor, CancellationToken cancellationToken)
    {
        context.Actors.Update(actor);
        await context.SaveChangesAsync(cancellationToken);

        return actor;
    }

    public async Task<Actor> Delete(Actor actor, CancellationToken cancellationToken)
    {
        context.Actors.Remove(actor);
        await context.SaveChangesAsync(cancellationToken);

        return actor;
    }
}
