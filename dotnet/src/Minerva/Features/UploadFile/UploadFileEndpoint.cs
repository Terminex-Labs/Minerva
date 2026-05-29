using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Minerva.Features.UploadFile
{
    public static class UploadFileEndpoint
    {
        public static void MapUploadFile(this IEndpointRouteBuilder app)
        {
            app.MapPost("/files", async ([FromForm] UploadFileRequest request, [FromServices] IMediator mediator, CancellationToken cancellationToken = default) =>
            {
                if (request.File is null || request.File.Length == 0)
                    return Results.BadRequest("Файл не выбран или пуст");

                var command = new UploadFileCommand
                    (
                        request.BucketName, 
                        request.AdditionalName, 
                        request.SubFolder, 
                        request.File.OpenReadStream(), 
                        request.File.FileName, 
                        request.File.ContentType
                    );

                var result = await mediator.Send(command, cancellationToken);

                return Results.Ok(result);
            }).DisableAntiforgery();
        }
    }
}
