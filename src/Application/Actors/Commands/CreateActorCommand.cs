using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Actors.Exceptions;
using Domain.Actors;
using MediatR;

namespace Application.Actors.Commands;

public record CreateActorCommand : IRequest<Result<Actor, ActorException>>
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required DateTime BirthDate { get; init; }
}

public class CreateActorCommandHandler(
    IActorRepository actorRepository)
    : IRequestHandler<CreateActorCommand, Result<Actor, ActorException>>
{
    public async Task<Result<Actor, ActorException>> Handle(CreateActorCommand request, CancellationToken cancellationToken)
    {
        // Перевірка, чи існує актор з таким самим ім'ям та прізвищем
        var existingActor = await actorRepository.GetByFullName(request.FirstName, request.LastName, cancellationToken);

        return await existingActor.Match(
            actor => Task.FromResult<Result<Actor, ActorException>>(new ActorAlreadyExistsException(actor.Id)),
            async () => await CreateEntity(request.FirstName, request.LastName, request.BirthDate, cancellationToken)
        );
    }

    private async Task<Result<Actor, ActorException>> CreateEntity(
        string firstName,
        string lastName,
        DateTime birthDate,
        CancellationToken cancellationToken)
    {
        try
        {
            // Створення нового актора
            var actor = Actor.New(ActorId.New(), firstName, lastName, birthDate);

            return await actorRepository.Add(actor, cancellationToken);
        }
        catch (Exception exception)
        {
            return new ActorUnknownException(ActorId.Empty(), exception);
        }
    }
}