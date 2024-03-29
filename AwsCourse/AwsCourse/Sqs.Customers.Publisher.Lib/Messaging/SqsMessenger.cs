﻿using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Options;
using Sqs.Customers.Publisher.Lib.Messaging.Settings;

namespace Sqs.Customers.Publisher.Lib.Messaging;

public class SqsMessenger : ISqsMessenger
{
    private readonly IAmazonSQS _sqs;
    private readonly IOptions<QueueSettings> _queueSettings;
    private string? _queueUrl;

    public SqsMessenger(IAmazonSQS sqs, IOptions<QueueSettings> queueSettings)
    {
        _sqs = sqs;
        _queueSettings = queueSettings;
    }

    public async Task<SendMessageResponse> SendMessageAsync<T>(T message)
    {
        var queueUrl = await GetQueueUrlAsync();

        var sendMessageRequest = new SendMessageRequest
        {
            QueueUrl = queueUrl,
            MessageBody = JsonSerializer.Serialize(message),
            MessageAttributes = new Dictionary<string, MessageAttributeValue>
            {
                { "MessageType", new MessageAttributeValue { DataType = "String", StringValue = typeof(T).Name } }
            }
        };

        var response = await _sqs.SendMessageAsync(sendMessageRequest);
        return response;
    }

    private async ValueTask<string> GetQueueUrlAsync()
    {
        return _queueUrl ??= (await _sqs.GetQueueUrlAsync(_queueSettings.Value.Name)).QueueUrl;
    }
}