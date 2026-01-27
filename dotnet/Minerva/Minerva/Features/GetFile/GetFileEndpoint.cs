using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Minerva.Features.GetFile
{
    public static class GetFileEndpoint
    {
        public static void MapGetFile(this IEndpointRouteBuilder app)
        {
            app.MapGet("/files/{bucketName}/{*objectPath}", async ([FromRoute] string bucketName, [FromRoute] string objectPath, [FromServices] IMediator mediator, CancellationToken ct) =>
            {
                try
                {
                    var query = new GetFileQuery(bucketName, objectPath);

                    var result = await mediator.Send(query, ct);

                    return Results.File
                        (
                            result.Content,
                            contentType: result.ContentType,
                            fileDownloadName: result.FileName
                        );
                }
                catch (FileNotFoundException ex)
                {
                    return Results.NotFound(ex.Message);
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message); // 500 Internal Server Error
                }
            }).WithTags("Files");
        }
    }
}
