using MediatR;

namespace Minerva.Features.DeleteFile
{
    public sealed record DeleteFileCommand(string BucketName, string ObjectPath) : IRequest;
}
