using MediatR;
using Microsoft.Extensions.Options;
using Minerva.Common.Minio;
using Minio;
using Minio.DataModel.Args;

namespace Minerva.Features.UploadFile
{
    public class UploadFileHandler(IMinioClient minioClient, IOptions<MinioOptions> options) : IRequestHandler<UploadFileCommand, UploadFileResponse>
    {
        private readonly IMinioClient _minioClient = minioClient;
        private readonly MinioOptions _options = options.Value;

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

                //var uniqueFileName = $"{Guid.NewGuid()}-{request.AdditionalName}-{request.FileName}";

                #region ToDo
                // TODO : Сделать нормальную проверку на '/' и повторяющиеся буквы

                //char currentChar;

                //foreach (var item in request.SubFolder)
                //{
                //    currentChar = item;

                //    if (currentChar == item)
                //        throw new ArgumentException("Некоректный путь!");
                //}
                #endregion

                //var cleanFolder = request.SubFolder?.Trim('/').Replace("//", "/");

                //var objectName = string.IsNullOrWhiteSpace(cleanFolder) ? uniqueFileName : $"{cleanFolder}/{uniqueFileName}";

                var args = new PutObjectArgs()
                    .WithBucket(request.BucketName.ToLower())
                    .WithObject(objectName)
                    .WithStreamData(request.Content)
                    .WithObjectSize(request.Content.Length)
                    .WithContentType(request.ContentType);

                await _minioClient.PutObjectAsync(args, cancellationToken);

                return new UploadFileResponse(objectName);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
