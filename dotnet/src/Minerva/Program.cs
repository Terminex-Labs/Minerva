using Amazon.S3;
using Amazon.Runtime;
using System.Reflection;
using Minerva.Features.GetFile;
using Minerva.Features.UploadFile;
using Minerva.Features.DeleteFile;
using Minerva.Features.UploadFiles;
using Minerva.Features.PresignedUrl;
using Minerva.Features.GetFileMetadata;

namespace Minerva
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            var awsConfig = builder.Configuration.GetSection("AWS");
            var serviceUrl = awsConfig["ServiceURL"];
            var accessKey = awsConfig["AccessKey"];
            var secretKey = awsConfig["SecretKey"];
            var forcePathStyle = bool.Parse(awsConfig["ForcePathStyle"] ?? "false");
            var useHttp = bool.Parse(awsConfig["UseHttp"] ?? "false");

            var credentials = new BasicAWSCredentials(accessKey, secretKey);

            builder.Services.AddSingleton<IAmazonS3>(sp => new AmazonS3Client(credentials, new AmazonS3Config
            {
                ServiceURL = serviceUrl,
                ForcePathStyle = forcePathStyle,
                UseHttp = useHttp
            }));

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            var app = builder.Build();

            app.MapGetFile();
            app.MapUploadFile();
            app.MapDeleteFile();
            app.MapUploadFiles();
            app.MapPresignedUrl();
            app.MapGetFileMetadata();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
