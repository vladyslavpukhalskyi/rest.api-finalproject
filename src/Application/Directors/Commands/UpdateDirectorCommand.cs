using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Directors.Exceptions;
using Domain.Directors;
using MediatR;

namespace Application.Directors.Commands;

public record UpdateDirectorCommand : IRequest<Result<Director, DirectorException>>
{
    public required Guid DirectorId { get; init; }
    public required string Name { get; init; }
    public required string Bio { get; init; }
    public required DateTime BirthDate { get; init; }
}

public class UpdateDirectorCommandHandler(IDirectorRepository directorRepository) : IRequestHandler<UpdateDirectorCommand, Result<Director, DirectorException>>
{
    public async Task<Result<Director, DirectorException>> Handle(UpdateDirectorCommand request, CancellationToken cancellationToken)
    {
        var directorId = new DirectorId(request.DirectorId);

        var existingDirector = await directorRepository.GetById(directorId, cancellationToken);

        return await existingDirector.Match(
            async d => await UpdateEntity(d, request.Name, request.Bio, request.BirthDate, cancellationToken),
            () => Task.FromResult<Result<Director, DirectorException>>(new DirectorNotFoundException(directorId)));
    }

    private async Task<Result<Director, DirectorException>> UpdateEntity(
        Director entity,
        string name,
        string bio,
        DateTime birthDate,
        CancellationToken cancellationToken)
    {
        try
        {
            entity.UpdateDetails(name, bio, birthDate);

            return await directorRepository.Update(entity, cancellationToken);
        }
        catch (Exception exception)
        {
            return new DirectorUnknownException(entity.Id, exception);
        }
    }
}