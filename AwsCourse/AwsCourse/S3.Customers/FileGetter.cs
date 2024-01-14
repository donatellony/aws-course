using System.Text;
using Amazon.S3;
using Amazon.S3.Model;

namespace S3.Customers;

public static class FileGetter
{
    public static async Task GetFileDemo()
    {
        var s3Client = new AmazonS3Client();
        var getObjectRequest = new GetObjectRequest
        {
            BucketName = BucketDemoSettings.BucketName,
            Key = BucketDemoSettings.S3Path
        };

        var response = await s3Client.GetObjectAsync(getObjectRequest);
        using var memoryStream = new MemoryStream();
        await response.ResponseStream.CopyToAsync(memoryStream);
        var text = Encoding.Default.GetString(memoryStream.ToArray());
        
        Console.WriteLine(text);
    }
}