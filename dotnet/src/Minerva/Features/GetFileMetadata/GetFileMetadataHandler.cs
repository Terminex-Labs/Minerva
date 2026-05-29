using MediatR;
using Amazon.S3;

namespace Minerva.Features.GetFileMetadata
{
    public class GetFileMetadataHandler(IAmazonS3 amazonS3) : IRequestHandler<GetFileMetadataQuery, GetFileMetadataResponse>
    {
        public async Task<GetFileMetadataResponse> Handle(GetFileMetadataQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var stat = await amazonS3.GetObjectMetadataAsync(request.BucketName, request.ObjectPath, cancellationToken);

                return new GetFileMetadataResponse
                    (
                        Path.GetFileName(request.ObjectPath),
                        stat.ContentType ?? "application/octet-stream",
                        stat.ContentLength
                    );
            }
            catch (Exception ex) when (ex is not FileNotFoundException)
            {
                throw new InvalidOperationException($"Ошибка при получении метаданных: {ex.Message}", ex);
            }
        }
    }
}