using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Sqs.Contracts;

var sqsClient = new AmazonSQSClient();

var customerCreated = new CustomerCreated
{
    Id = Guid.NewGuid(),
    FullName = "Yehor Voiko",
    Email = "yehorvoiko@example123123123.com",
    GitHubUsername = "yehorvoiko",
    DateOfBirth = new DateTime(2000, 1, 1)
};

var queueUrlResponse = await sqsClient.GetQueueUrlAsync("customers");

var sendMessageRequest = new SendMessageRequest
{
    QueueUrl = queueUrlResponse.QueueUrl,
    MessageBody = JsonSerializer.Serialize(customerCreated),
    MessageAttributes = new Dictionary<string, MessageAttributeValue>
    {
        {
            "MessageType", new MessageAttributeValue
            {
                DataType = "String",
                StringValue = nameof(CustomerCreated)
            }
        }
    }
};

var response = await sqsClient.SendMessageAsync(sendMessageRequest);

Console.WriteLine();