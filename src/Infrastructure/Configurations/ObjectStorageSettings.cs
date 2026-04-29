namespace Infrastructure.Configurations;

public class ObjectStorageSettings
{
    public string Endpoint { get; set; } = null!;
    public string AccessKey { get; set; } = null!;
    public string SecretKey { get; set; } = null!;
    public string BucketName { get; set; } = null!;
}