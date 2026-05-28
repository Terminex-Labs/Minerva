using Minerva.Common.Minio;
using Minerva.Features.DeleteFile;
using Minerva.Features.GetFile;
using Minerva.Features.GetFileMetadata;
using Minerva.Features.PresignedUrl;
using Minerva.Features.UploadFile;
using Minerva.Features.UploadFiles;
using Minio;

namespace Minerva
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            var minioConfig = builder.Configuration.GetSection("Minio");
            var endpoint = minioConfig["Endpoint"];
            var accessKey = minioConfig["AccessKey"];
            var secretKey = minioConfig["SecretKey"];
            var useSsl = bool.Parse(minioConfig["UseSsl"] ?? "false");

            // TODO : ���� �� ������������, �� ����� ����� �����������
            builder.Services.AddOptions<MinioOptions>()
                .Bind(builder.Configuration.GetSection(MinioOptions.SectionName));

            builder.Services.AddSingleton<IMinioClient>(sp =>
            {
                return new MinioClient()
                    .WithEndpoint(endpoint)
                    .WithCredentials(accessKey, secretKey)
                    .WithSSL(useSsl)
                    .Build();
            });

            builder.Services.AddMediatR(cfg => 
                cfg.RegisterServicesFromAssembly(typeof(Program).Assembly)
            );

            var app = builder.Build();

            app.MapUploadFile();
            app.MapGetFile();
            app.MapGetFileMetadata();
            app.MapPresignedUrl();
            app.MapDeleteFile();
            app.MapUploadFiles();

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
