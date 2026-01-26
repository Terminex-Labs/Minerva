using MediatR;
using Minio;
using Minio.DataModel.Args;

namespace Minerva.Features.GetFile
{
    public class GetFileHandler(IMinioClient minioClient) : IRequestHandler<GetFileQuery, GetFileResult>
    {
        private readonly IMinioClient _minioClient = minioClient;

        public async Task<GetFileResult> Handle(GetFileQuery request, CancellationToken cancellationToken)
        {
            var bucket = request.BucketName.ToLower();
            var objectName = request.ObjectPath.TrimStart('/');

            var statArgs = new StatObjectArgs()
                .WithBucket(bucket)
                .WithObject(objectName);

            var stat = await _minioClient.StatObjectAsync(statArgs, cancellationToken);

            var memoryStream = new MemoryStream();

            var getArgs = new GetObjectArgs()
                .WithBucket(bucket)
                .WithObject(objectName)
                .WithCallbackStream(stream => stream.CopyTo(memoryStream));

            await _minioClient.GetObjectAsync(getArgs, cancellationToken);

            memoryStream.Position = 0;

            return new GetFileResult(
                memoryStream,
                stat.ContentType,
                Path.GetFileName(objectName)
            );
        }
    }
}
