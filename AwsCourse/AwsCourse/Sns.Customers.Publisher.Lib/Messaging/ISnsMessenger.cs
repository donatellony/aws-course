using Amazon.SimpleNotificationService.Model;

namespace Sns.Customers.Publisher.Lib.Messaging;

public interface ISnsMessenger
{
    Task<PublishResponse> PublishMessageAsync<T>(T message);
}