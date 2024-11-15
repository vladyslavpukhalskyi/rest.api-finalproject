using Application.Files.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("files")]
[ApiController]
public class FilesController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Upload([FromForm] IFormFile imageFile, CancellationToken cancellationToken)
    {
        var input = new UploadFileCommand
        {
            FileName = imageFile.FileName,
            FileStream = imageFile.OpenReadStream()
        };

        var result = await sender.Send(input, cancellationToken);

        return Ok(result);
    }

    [HttpPost("multiple")]
    public async Task<ActionResult> UploadMany(
        [FromForm] IReadOnlyList<IFormFile> files,
        CancellationToken cancellationToken)
    {
        var filesNames = files.Select(f => f.FileName).ToArray();

        return Ok(filesNames);
    }
}