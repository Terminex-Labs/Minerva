using MediatR;
using Amazon.S3;
using Amazon.S3.Model;

namespace Minerva.Features.UploadFile
{
    public class UploadFileHandler(IAmazonS3 amazonS3) : IRequestHandler<UploadFileCommand, UploadFileResponse>
    {
        public async Task<UploadFileResponse> Handle(UploadFileCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var bucket = request.BucketName.ToLower();
                var cleanFileNameWithoutExtension = Path.GetFileNameWithoutExtension(request.FileName);
                var originalExtension = Path.GetExtension(request.FileName);
                var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd_HH-mm-ss");
                var uniqueFileName = $"{Guid.NewGuid()}_{timestamp}_{request.AdditionalName}_{cleanFileNameWithoutExtension}{originalExtension}";
                var cleanSubFolder = request.SubFolder?.Trim('/').Replace("//", "/");
                var objectName = string.IsNullOrWhiteSpace(cleanSubFolder)
                    ? uniqueFileName
                    : $"{cleanSubFolder}/{uniqueFileName}";

                var putRequest = new PutObjectRequest
                {
                    BucketName = bucket,
                    Key = objectName,
                    InputStream = request.Content,
                    ContentType = request.ContentType,
                };

                var putResponse = await amazonS3.PutObjectAsync(putRequest);
                
                return new UploadFileResponse(objectName);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
