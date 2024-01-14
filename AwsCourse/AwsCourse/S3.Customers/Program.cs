using Amazon.S3;
using Amazon.S3.Model;

// Just a VERY quick demo, the real project shouldn't be built like that
const string filePath = "./example.jpeg";
const string bucketName = "test-bucket-name";

var s3Client = new AmazonS3Client();
await using var inputStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

var s3Path = $"images/{Path.GetFileName(filePath)}";
var putObjectRequest = new PutObjectRequest
{
    BucketName = bucketName,
    Key = s3Path,
    ContentType = "image/jpeg",
    InputStream = inputStream
};

await s3Client.PutObjectAsync(putObjectRequest);