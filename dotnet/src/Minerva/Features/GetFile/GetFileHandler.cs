using MediatR;
using Amazon.S3;
using Amazon.S3.Model;

namespace Minerva.Features.GetFile
{
    public class GetFileHandler(IAmazonS3 amazonS3) : IRequestHandler<GetFileQuery, GetFileResult>
    {
        public async Task<GetFileResult> Handle(GetFileQuery request, CancellationToken cancellationToken)
        {
            var bucket = request.BucketName.ToLower();
            var objectName = request.ObjectPath.TrimStart('/');

            var getRequest = new GetObjectRequest
            {
                BucketName = bucket,
                Key = objectName
            };

            using var response = await amazonS3.GetObjectAsync(getRequest, cancellationToken);
            var memoryStream = new MemoryStream();

            await response.ResponseStream.CopyToAsync(memoryStream, cancellationToken);

            memoryStream.Position = 0;

            return new GetFileResult
            (
                memoryStream,
                response.Headers.ContentType ?? "application/octet-stream",
                Path.GetFileName(objectName)
            );
        }
    }
}
