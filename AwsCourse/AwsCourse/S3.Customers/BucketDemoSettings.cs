namespace S3.Customers;

public static class BucketDemoSettings
{
    public const string FilePath = "./example.jpeg";
    public const string BucketName = "test-bucket-name";
    public static readonly string S3Path = $"images/{Path.GetFileName(FilePath)}";
}