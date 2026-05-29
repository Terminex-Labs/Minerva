using MediatR;
using Amazon.S3;
using Amazon.S3.Model;

namespace Minerva.Features.PresignedUrl
{
    public sealed class PresignedUrlHandler(IAmazonS3 amazonS3) : IRequestHandler<PresignedUrlCommand, PresignedUrlResponse>
    {
        public async Task<PresignedUrlResponse> Handle(PresignedUrlCommand request, CancellationToken cancellationToken)
        {
            var putRequest = new GetPreSignedUrlRequest
            {
                BucketName = request.BucketName,
                Key = request.FileName,
                Expires = DateTime.UtcNow.Add(TimeSpan.Parse($"{3600 * 24}")),
                Verb = HttpVerb.GET,
                ContentType = InferMimeType(request.FileName),
                Protocol = Protocol.HTTP
            };

            var url = await amazonS3.GetPreSignedURLAsync(putRequest);

            return new PresignedUrlResponse(url);
        }

        private static string InferMimeType(string key)
        {
            var ext = Path.GetExtension(key).ToLowerInvariant();
            return ext switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".webp" => "image/webp",
                ".mp4" => "video/mp4",
                ".webm" => "video/webm",
                ".pdf" => "application/pdf",
                _ => "application/octet-stream"
            };
        }

    }
}