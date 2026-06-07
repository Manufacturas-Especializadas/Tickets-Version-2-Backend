namespace Application.Common.Interfaces
{
    public record FileUploadDto(string FileName, string ContentType, Stream Content);

    public interface IFileStorageService
    {
        Task<string> UploadFileAsync(FileUploadDto file, CancellationToken cancellationToken);
    }
}