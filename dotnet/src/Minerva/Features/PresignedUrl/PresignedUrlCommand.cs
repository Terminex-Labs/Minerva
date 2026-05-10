using MediatR;

namespace Minerva.Features.PresignedUrl
{
    public sealed record PresignedUrlCommand(string BucketName, string FileName) : IRequest<PresignedUrlResponse>;
}