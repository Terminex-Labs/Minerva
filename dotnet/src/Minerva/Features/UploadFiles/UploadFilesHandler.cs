using MediatR;
using Amazon.S3;
using Amazon.S3.Model;
using Minerva.Features.UploadFile;

namespace Minerva.Features.UploadFiles
{
    public class UploadFilesHandler(IAmazonS3 amazonS3) : IRequestHandler<UploadFilesCommand, IReadOnlyList<UploadFileResponse>>
    {
        public async Task<IReadOnlyList<UploadFileResponse>> Handle(UploadFilesCommand request, CancellationToken cancellationToken)
        {
            var uploadedFiles = new List<UploadFileResponse>();

            try
            {
                foreach (var file in request.Files)
                {
                    var bucket = file.BucketName;

                    var cleanSubFolder = file.SubFolder?.Trim('/').Replace("//", "/");
                    var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd_HH-mm-ss");


                    var cleanFileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
                    var originalExtension = Path.GetExtension(file.FileName);
                    var uniqueFileName = $"{Guid.NewGuid()}_{timestamp}_{file.AdditionalName}_{cleanFileNameWithoutExtension}{originalExtension}";
                    var objectName = string.IsNullOrWhiteSpace(cleanSubFolder)
                        ? uniqueFileName
                        : $"{cleanSubFolder}/{uniqueFileName}";

                    var putRequest = new PutObjectRequest
                    {
                        BucketName = bucket,
                        Key = objectName,
                        InputStream = file.Content,
                        ContentType = file.ContentType,
                    };

                    await amazonS3.PutObjectAsync(putRequest, cancellationToken);

                    uploadedFiles.Add(new UploadFileResponse(objectName));
                }
            }
            catch (Exception)
            {

                throw;
            }

            return uploadedFiles;
        }
    }
}
