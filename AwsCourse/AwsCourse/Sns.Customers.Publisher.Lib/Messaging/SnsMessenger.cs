using System.Text.Json;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Microsoft.Extensions.Options;
using Sns.Customers.Publisher.Lib.Messaging.Settings;

namespace Sns.Customers.Publisher.Lib.Messaging;

public class SnsMessenger : ISnsMessenger
{
    private readonly IAmazonSimpleNotificationService _sns;
    private readonly IOptions<TopicSettings> _topicSettings;
    private string? _topicArn;

    public SnsMessenger(IAmazonSimpleNotificationService sns, IOptions<TopicSettings> topicSettings)
    {
        _sns = sns;
        _topicSettings = topicSettings;
    }

    public async Task<PublishResponse> PublishMessageAsync<T>(T message)
    {
        var topicArn = await GetTopicArnAsync();

        var sendMessageRequest = new PublishRequest
        {
            TopicArn = topicArn,
            Message = JsonSerializer.Serialize(message),
            MessageAttributes = new Dictionary<string, MessageAttributeValue>
            {
                { "MessageType", new MessageAttributeValue { DataType = "String", StringValue = typeof(T).Name } }
            }
        };

        var response = await _sns.PublishAsync(sendMessageRequest);
        return response;
    }

    private async ValueTask<string> GetTopicArnAsync()
    {
        return _topicArn ??= (await _sns.FindTopicAsync(_topicSettings.Value.Name)).TopicArn;
    }
}