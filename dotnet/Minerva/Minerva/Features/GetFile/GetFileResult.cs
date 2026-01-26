namespace Minerva.Features.GetFile
{
    public sealed record GetFileResult(Stream Content, string ContentType, string FileName);
}
