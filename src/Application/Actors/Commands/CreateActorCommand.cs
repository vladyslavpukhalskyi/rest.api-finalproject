﻿using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Actors.Exceptions;
using Domain.Actors;
using MediatR;

namespace Application.Actors.Commands
{
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
            // Check if the actor already exists by their full name
            var existingActor = await actorRepository.GetByFullName(request.FirstName, request.LastName, cancellationToken);

            return await existingActor.Match<Task<Result<Actor, ActorException>>>(
                actor => Task.FromResult<Result<Actor, ActorException>>(new ActorAlreadyExistsException(actor.Id)),
                async () => await CreateActorEntity(request.FirstName, request.LastName, request.BirthDate, cancellationToken)
            );
        }

        private async Task<Result<Actor, ActorException>> CreateActorEntity(
            string firstName,
            string lastName,
            DateTime birthDate,
            CancellationToken cancellationToken)
        {
            try
            {
                var actorId = ActorId.New();
                var actor = Actor.New(actorId, firstName, lastName, birthDate);

                // Add the actor to the repository
                return await actorRepository.Add(actor, cancellationToken);
            }
            catch (Exception exception)
            {
                return new ActorUnknownException(ActorId.Empty(), exception);
            }
        }
    }
}