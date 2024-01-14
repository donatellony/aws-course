using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

var secretsManager = new AmazonSecretsManagerClient();

var request = new GetSecretValueRequest
{
    SecretId = "ApiKey"
};

var response = await secretsManager.GetSecretValueAsync(request);

Console.WriteLine(response.SecretString);

var describeSecretRequest = new DescribeSecretRequest
{
    SecretId = "ApiKey",
};
var describeResponse = await secretsManager.DescribeSecretAsync(describeSecretRequest);

Console.WriteLine();