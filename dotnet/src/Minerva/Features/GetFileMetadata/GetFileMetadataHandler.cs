using MediatR;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;

namespace Minerva.Features.GetFileMetadata
{
    public class GetFileMetadataHandler(MinioClient minioClient) : IRequestHandler<GetFileMetadataQuery, GetFileMetadataResponse>
    {
        private readonly MinioClient _minioClient = minioClient;

        public async Task<GetFileMetadataResponse> Handle(GetFileMetadataQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var stat = await _minioClient.StatObjectAsync(new StatObjectArgs().WithBucket(request.BucketName).WithObject(request.ObjectPath), cancellationToken);

                return new GetFileMetadataResponse
                    (
                        Path.GetFileName(request.ObjectPath),
                        stat.ContentType ?? "application/octet-stream",
                        stat.Size
                    );
            }
            catch (ObjectNotFoundException)
            {
                throw new FileNotFoundException($"Файл '{request.ObjectPath}' не найден в бакете '{request.BucketName}'");
            }
            catch (Exception ex) when (ex is not FileNotFoundException)
            {
                throw new InvalidOperationException($"Ошибка при получении метаданных: {ex.Message}", ex);
            }
        }
    }
}