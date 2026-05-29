using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Minerva.Features.DeleteFile
{
    public static class DeleteFileEndpoint
    {
        public static void MapDeleteFile(this IEndpointRouteBuilder app)
        {
            app.MapDelete("/files/{bucketName}/{*objectPath}", async (string bucketName, string objectPath, [FromServices] IMediator mediator, CancellationToken ct) =>
            {
                await mediator.Send(new DeleteFileCommand(bucketName, objectPath), ct);
                
                return Results.NoContent();
            }).WithTags("Files");
        }
    }
}
