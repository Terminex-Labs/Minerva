using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Minerva.Features.UploadFiles
{
    public static class UploadFilesEndpoint
    {
        public static void MapUploadFiles(this IEndpointRouteBuilder app)
        {
            app.MapPost("/files/batch", async ([FromForm] UploadFilesRequest request, [FromServices] IMediator mediator, CancellationToken cancellationToken) =>
            {
                if (request.Files == null || !request.Files.Any())
                    return Results.BadRequest("Не выбрано ни одного файла!");

                var validFiles = request.Files.Where(file => file != null && file.File.Length > 0).ToList();

                if (!validFiles.Any())
                    return Results.BadRequest("Все файлы пусты или не выбраны!");

                var command = new UploadFilesCommand
                    (
                        [.. request.Files.Select
                            (file => new FileUploadData
                                (
                                    file.BucketName, 
                                    file.AdditionalName, 
                                    file.SubFolder, 
                                    file.File.OpenReadStream(), 
                                    file.File.FileName, 
                                    file.File.ContentType
                                )
                            )]
                    );

                var result = await mediator.Send(command, cancellationToken);

                if (!result.Any())
                    return Results.Problem("Не удалось загрузить ни одного файла!");

                return Results.Ok(result);
            }).WithTags("Files").DisableAntiforgery();
        }
    }
}
