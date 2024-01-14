using Amazon.S3;
using Amazon.S3.Model;

namespace S3.Customers;

public static class FilePutter
{
    public static async Task PutFileDemo()
    {
        // Just a VERY quick demo, the real project shouldn't be built like that
        const string filePath = BucketDemoSettings.FilePath;
        const string bucketName = BucketDemoSettings.BucketName;

        var s3Client = new AmazonS3Client();
        await using var inputStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

        var s3Path = BucketDemoSettings.S3Path;
        var putObjectRequest = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = s3Path,
            ContentType = "image/jpeg",
            InputStream = inputStream
        };

        await s3Client.PutObjectAsync(putObjectRequest);
    }
}