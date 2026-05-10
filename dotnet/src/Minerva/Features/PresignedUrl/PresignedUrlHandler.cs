using Minio;
using MediatR;
using Minio.DataModel.Args;

namespace Minerva.Features.PresignedUrl
{
    public sealed class PresignedUrlHandler(IMinioClient minioClient) : IRequestHandler<PresignedUrlCommand, PresignedUrlResponse>
    {
        private readonly IMinioClient _minioClient = minioClient;

        public async Task<PresignedUrlResponse> Handle(PresignedUrlCommand request, CancellationToken cancellationToken)
        {
            var settings = new PresignedGetObjectArgs().WithBucket(request.BucketName).WithObject(request.FileName).WithExpiry(3600 * 24);

            var url = await _minioClient.PresignedGetObjectAsync(settings);

            return new PresignedUrlResponse(url);
        }
    }
}