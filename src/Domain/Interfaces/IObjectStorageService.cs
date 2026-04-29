namespace Domain.Interfaces;

public interface IObjectStorageService
{
    Task<string> UploadToStorageAsync(Stream stream, string fileName, string contentType);
    Task<string> GetDownloadUrlAsync(string? fileUrl, int expiryInSeconds = 900);
}