using MediatR;
using Minio;
using Minio.DataModel.Args;

namespace Minerva.Features.DeleteFile
{
    public class DeleteFileHandler(IMinioClient minioClient) : IRequestHandler<DeleteFileCommand>
    {
        private readonly IMinioClient _minioClient = minioClient;

        public async Task Handle(DeleteFileCommand request, CancellationToken cancellationToken)
        {
            var bucket = request.BucketName.ToLower();
            var objectPath = request.ObjectPath;

            var args = new RemoveObjectArgs()
                .WithBucket(bucket)
                .WithObject(objectPath);

            await _minioClient.RemoveObjectAsync(args, cancellationToken);
        }
    }
}
