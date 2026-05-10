using MediatR;
using Microsoft.AspNetCore.Mvc;
using Minio.Exceptions;

namespace Minerva.Features.DeleteFile
{
    public static class DeleteFileEndpoint
    {
        public static void MapDeleteFile(this IEndpointRouteBuilder app)
        {
            app.MapDelete("/files/{bucketName}/{*objectPath}", async (string bucketName, string objectPath, [FromServices] IMediator mediator, CancellationToken ct) =>
            {
                try
                {
                    await mediator.Send(new DeleteFileCommand(bucketName, objectPath), ct);

                    // 204 No Content — стандарт для успешного удаления
                    return Results.NoContent();
                }
                catch (BucketNotFoundException ex) // Пример обработки конкретной ошибки
                {
                    return Results.NotFound(ex.Message);
                }
                catch (Exception ex)
                {
                    // Логировать ошибку
                    return Results.Problem(ex.Message); // 500 Internal Server Error
                }
            }).WithTags("Files");
        }
    }
}
