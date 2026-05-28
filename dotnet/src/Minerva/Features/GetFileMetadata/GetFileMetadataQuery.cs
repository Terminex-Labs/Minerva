using MediatR;

namespace Minerva.Features.GetFileMetadata
{
    public sealed record GetFileMetadataQuery(string BucketName, string ObjectPath) : IRequest<GetFileMetadataResponse>;
}