using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Minerva.Features.GetFileMetadata
{
    public static class GetFileMetadataEndpoint
    {
        public static void MapGetFileMetadata(this IEndpointRouteBuilder app)
        {
            app.MapPost("files/meta-data", async ([FromBody] GetFileMetadataQuery query, [FromServices] IMediator mediator, CancellationToken ct) =>
            {
                try
                {
                    var result = await mediator.Send(query, ct);

                    return Results.Json(result, contentType: "application/json");
                }
                catch (FileNotFoundException ex)
                {
                    return Results.NotFound(ex.Message);
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            }).Accepts<GetFileMetadataQuery>("application/json")
            .WithTags("Files").WithName("GetFileMetadata")
            .WithSummary("Получить метаданные файла без скачивания");
        }
    }
}