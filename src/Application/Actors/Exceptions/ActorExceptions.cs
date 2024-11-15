using Domain.Actors;

namespace Application.Actors.Exceptions;

public abstract class ActorException(Guid actorId, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public Guid ActorId { get; } = actorId;
}

public class ActorNotFoundException(Guid actorId) 
    : ActorException(actorId, $"Actor with id: {actorId} not found");

public class ActorAlreadyExistsException(Guid actorId) 
    : ActorException(actorId, $"Actor already exists: {actorId}");

public class ActorUnknownException(Guid actorId, Exception innerException)
    : ActorException(actorId, $"Unknown exception for the actor with id: {actorId}", innerException);

public class ActorNameInvalidException(string name) 
    : ActorException(Guid.Empty, $"Invalid name for actor: '{name}'");