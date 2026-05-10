namespace Minerva.Features.PresignedUrl
{
    public sealed record PresignedUrlRequest(string BucketName, string FileName);
}