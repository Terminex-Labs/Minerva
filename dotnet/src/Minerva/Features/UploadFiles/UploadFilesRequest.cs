namespace Minerva.Features.UploadFiles
{
    public sealed record UploadFilesRequest(List<UploadFilesDataRequest> Files);

    public sealed record UploadFilesDataRequest(string BucketName, string AdditionalName, string? SubFolder, IFormFile File);
}
