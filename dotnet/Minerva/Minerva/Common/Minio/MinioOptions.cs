using System.ComponentModel.DataAnnotations;

namespace Minerva.Common.Minio
{
    public class MinioOptions
    {
        public const string SectionName = "Minio";

        [Required, Url]
        public string Endpoint { get; set; } = string.Empty;

        [Required]
        public string AccessKey { get; set; } = string.Empty;

        [Required]
        public string SecretKey { get; set; } = string.Empty;

        [Required]
        public string BucketName { get; set; } = string.Empty;

        public bool UseSSL { get; set; } = false;

        // Можно добавить регион, если планируете переезд в AWS S3
        public string Region { get; set; } = "us-east-1";
    }
}
