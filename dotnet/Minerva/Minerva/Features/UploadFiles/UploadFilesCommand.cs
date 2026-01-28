using MediatR;
using Minerva.Features.UploadFile;

namespace Minerva.Features.UploadFiles
{
    public sealed record UploadFilesCommand(List<FileUploadData> Files) : IRequest<IReadOnlyList<UploadFileResponse>>;

    public sealed record FileUploadData(string BucketName, string AdditionalName, string? SubFolder, Stream Content, string FileName, string ContentType);
}
