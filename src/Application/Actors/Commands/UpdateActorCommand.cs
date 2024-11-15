using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Actors.Exceptions;
using Domain.Actors;
using MediatR;

namespace Application.Actors.Commands;

public record UpdateActorCommand : IRequest<Result<Actor, ActorException>>
{
    public required Guid ActorId { get; init; }
    public required string Name { get; init; }
    public required DateTime BirthDate { get; init; }
}

public class UpdateActorCommandHandler(IActorRepository actorRepository) : IRequestHandler<UpdateActorCommand, Result<Actor, ActorException>>
{
    public async Task<Result<Actor, ActorException>> Handle(UpdateActorCommand request, CancellationToken cancellationToken)
    {
        var actorId = new ActorId(request.ActorId);

        var existingActor = await actorRepository.GetById(actorId, cancellationToken);

        return await existingActor.Match(
            async a => await UpdateEntity(a, request.Name, request.BirthDate, cancellationToken),
            () => Task.FromResult<Result<Actor, ActorException>>(new ActorNotFoundException(actorId)));
    }

    private async Task<Result<Actor, ActorException>> UpdateEntity(
        Actor entity,
        string name,
        DateTime birthDate,
        CancellationToken cancellationToken)
    {
        try
        {
            entity.UpdateDetails(name, birthDate);

            return await actorRepository.Update(entity, cancellationToken);
        }
        catch (Exception exception)
        {
            return new ActorUnknownException(entity.Id, exception);
        }
    }
}