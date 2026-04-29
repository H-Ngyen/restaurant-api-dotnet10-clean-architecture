using Domain.Interfaces;
using Infrastructure.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace Infrastructure.Services;

public class ObjectStorageService(IMinioClient minioClient,
    IOptions<ObjectStorageSettings> fileStorageSettings) : IObjectStorageService
{
    private readonly ObjectStorageSettings _settings = fileStorageSettings.Value;

    public async Task<string> UploadToStorageAsync(Stream stream, string fileName, string contentType)
    {
        var putObjectArgs = new PutObjectArgs()
            .WithBucket(_settings.BucketName)
            .WithObject(fileName)
            .WithStreamData(stream)
            .WithObjectSize(stream.Length)
            .WithContentType(contentType);

        await minioClient.PutObjectAsync(putObjectArgs);

        return $"{_settings.BucketName}/{fileName}";
    }

    public async Task<string> GetDownloadUrlAsync(string? fileUrl, int expiryInSeconds = 900)
    {
        if (string.IsNullOrWhiteSpace(fileUrl)) return string.Empty;

        var parts = fileUrl.Split('/');
        var objectName = parts.Length > 1 ? parts[^1] : fileUrl;

        var args = new PresignedGetObjectArgs()
            .WithBucket(_settings.BucketName)
            .WithObject(objectName)
            .WithExpiry(expiryInSeconds); // 15p

        return await minioClient.PresignedGetObjectAsync(args);
    }
}