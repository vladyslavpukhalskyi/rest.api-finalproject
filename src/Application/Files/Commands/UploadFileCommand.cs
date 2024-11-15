using Domain.Faculties;
using MediatR;

namespace Application.Files.Commands;

public record UploadFileCommand : IRequest<string>
{
    public string FileName { get; init; }
    public Stream FileStream { get; init; }
}

public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, string>
{
    public async Task<string> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        var entity = FacultyImage.New(FacultyImageId.New(), FacultyId.New());

        return await UploadImages(entity, request.FileStream, request.FileName, cancellationToken);
    }

    private async Task<string> UploadImages(
        FacultyImage imageEntity,
        Stream fileStream,
        string fileName,
        CancellationToken cancellationToken)
    {
        // Save to storage, get read stream
        var readStream = imageEntity.FilePath;

        var resultStream = new MemoryStream();
        var resultStreamSecond = new MemoryStream();

        await fileStream.CopyToAsync(resultStream, cancellationToken);
        await resultStream.FlushAsync(cancellationToken);
        resultStream.Close();

        fileStream.Position = 0;
        await fileStream.CopyToAsync(resultStreamSecond, cancellationToken);
        await resultStreamSecond.FlushAsync(cancellationToken);
        resultStreamSecond.Close();

        return fileName;
    }
}