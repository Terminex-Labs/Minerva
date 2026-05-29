using MediatR;
using Amazon.S3;
using Amazon.S3.Model;

namespace Minerva.Features.DeleteFile
{
    public class DeleteFileHandler(IAmazonS3 amazonS3) : IRequestHandler<DeleteFileCommand>
    {
        public async Task Handle(DeleteFileCommand request, CancellationToken cancellationToken)
        {
            var bucket = request.BucketName.ToLower();
            var objectPath = request.ObjectPath.TrimStart('/');

            try
            {
                var args = new DeleteObjectRequest
                {
                    BucketName = bucket,
                    Key = objectPath
                };

                await amazonS3.DeleteObjectAsync(args, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Ошибка s3-хранилища при удалении файла: {ex.Message}", ex);
            }
        }
    }
}
