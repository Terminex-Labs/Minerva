using MediatR;

namespace Minerva.Features.DeleteFile
{
    public static class DeleteFileEndpoint
    {
        public static void MapDeleteFile(this IEndpointRouteBuilder app)
        {
            app.MapDelete("/files/{bucketName}/{*objectPath}", async (string bucketName, string objectPath, ISender mediator, CancellationToken ct) =>
            {
                await mediator.Send(new DeleteFileCommand(bucketName, objectPath), ct);

                // 204 No Content — стандарт для успешного удаления
                return Results.NoContent();
            })
            .WithTags("Files");
        }
    }
}
