using MediatR;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;

namespace Minerva.Features.DeleteFile
{
    public class DeleteFileHandler(IMinioClient minioClient) : IRequestHandler<DeleteFileCommand>
    {
        private readonly IMinioClient _minioClient = minioClient;

        public async Task Handle(DeleteFileCommand request, CancellationToken cancellationToken)
        {
            var bucket = request.BucketName.ToLower();
            var objectPath = request.ObjectPath.TrimStart('/');

            try
            {
                var args = new RemoveObjectArgs()
                    .WithBucket(bucket)
                    .WithObject(objectPath);

                await _minioClient.RemoveObjectAsync(args, cancellationToken);
            }
            catch (MinioException ex)
            {
                throw new ApplicationException($"Ошибка Minio при удалении файла: {ex.Message}", ex);
            }
        }
    }
}
