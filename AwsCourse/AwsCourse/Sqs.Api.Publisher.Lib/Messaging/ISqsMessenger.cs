using Amazon.SQS.Model;

namespace Sqs.Api.Publisher.Lib.Messaging;

public interface ISqsMessenger
{
    Task<SendMessageResponse> SendMessageAsync<T>(T message);
}