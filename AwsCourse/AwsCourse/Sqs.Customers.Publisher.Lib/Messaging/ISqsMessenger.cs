using Amazon.SQS.Model;

namespace Sqs.Customers.Publisher.Lib.Messaging;

public interface ISqsMessenger
{
    Task<SendMessageResponse> SendMessageAsync<T>(T message);
}