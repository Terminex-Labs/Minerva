namespace Minerva.Features.GetFileMetadata
{
    public sealed record GetFileMetadataResponse(string FileName, string ContentType, long Size);
}