using MediatR;

namespace Minerva.Features.UploadFile
{
    public sealed record UploadFileCommand(string BucketName, string AdditionalName, string? SubFolder, Stream Content, string FileName, string ContentType) : IRequest<UploadFileResponse>;
}
