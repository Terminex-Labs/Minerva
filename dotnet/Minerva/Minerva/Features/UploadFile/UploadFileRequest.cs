namespace Minerva.Features.UploadFile
{
    public sealed record UploadFileRequest(string BucketName, string AdditionalName, string? SubFolder, IFormFile File);
}
