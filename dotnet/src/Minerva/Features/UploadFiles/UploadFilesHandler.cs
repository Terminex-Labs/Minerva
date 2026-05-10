using MediatR;
using Minerva.Features.UploadFile;
using Minio;
using Minio.DataModel.Args;

namespace Minerva.Features.UploadFiles
{
    public class UploadFilesHandler(IMinioClient minioClient) : IRequestHandler<UploadFilesCommand, IReadOnlyList<UploadFileResponse>>
    {
        private readonly IMinioClient _minioClient = minioClient;

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

                    var putObjectArgs = new PutObjectArgs()
                        .WithBucket(bucket)
                        .WithObject(objectName)
                        .WithStreamData(file.Content)
                        .WithObjectSize(file.Content.Length)
                        .WithContentType(file.ContentType);

                    await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);

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
