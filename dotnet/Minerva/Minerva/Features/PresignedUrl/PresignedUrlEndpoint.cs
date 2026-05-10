using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Minerva.Features.PresignedUrl
{
    public static class PresignedUrlEndpoint
    {
        public static void MapPresignedUrl(this IEndpointRouteBuilder app)
        {
            app.MapPost("/presigned-url", async ([FromBody] PresignedUrlRequest request, [FromServices] IMediator mediator, CancellationToken cancellationToken = default) =>
            {
                var command = new PresignedUrlCommand(request.BucketName, request.FileName);

                var result = await mediator.Send(command, cancellationToken);

                return Results.Ok(result);
            });
        }
    }
}