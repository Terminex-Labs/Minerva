using MediatR;

namespace Minerva.Features.GetFile
{
    public sealed record GetFileQuery(string BucketName, string ObjectPath) : IRequest<GetFileResult>;
}
