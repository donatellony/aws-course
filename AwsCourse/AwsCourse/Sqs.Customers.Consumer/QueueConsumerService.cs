using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using MediatR;
using Microsoft.Extensions.Options;
using Sqs.Customers.Consumer.Messaging.Settings;

namespace Sqs.Customers.Consumer;

public class QueueConsumerService : BackgroundService
{
    private readonly IAmazonSQS _sqs;
    private readonly IOptions<QueueSettings> _queueSettings;
    private string? _queueUrl;
    private readonly ISender _sender;
    private readonly ILogger<QueueConsumerService> _logger;

    public QueueConsumerService(IAmazonSQS sqs, IOptions<QueueSettings> queueSettings, ISender sender,
        ILogger<QueueConsumerService> logger)
    {
        _sqs = sqs;
        _queueSettings = queueSettings;
        _sender = sender;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var queueUrl = await GetQueueUrl(stoppingToken);

        var receiveMessageRequest = new ReceiveMessageRequest
        {
            QueueUrl = queueUrl,
            AttributeNames = ["All"], // NOT RECOMMENDED TO LEAVE "ALL". It is there for the example purposes only!
            MessageAttributeNames = ["All"], // NOT RECOMMENDED TO LEAVE "ALL". It is there for the example purposes only!
            MaxNumberOfMessages = 1
        };

        while (!stoppingToken.IsCancellationRequested)
        {
            var response = await _sqs.ReceiveMessageAsync(receiveMessageRequest, stoppingToken);
            foreach (var message in response.Messages)
            {
                var typedMessage = TryGetDeserializedMessage(message);
                if (typedMessage is null)
                {
                    continue;
                }

                try
                {
                    await _sender.Send(typedMessage, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while sending message to handler");
                    continue;
                }

                await _sqs.DeleteMessageAsync(queueUrl, message.ReceiptHandle, stoppingToken);
            }

            await Task.Delay(1000, stoppingToken);
        }
    }

    private async Task<string> GetQueueUrl(CancellationToken stoppingToken)
    {
        return _queueUrl ??= (await _sqs.GetQueueUrlAsync(_queueSettings.Value.Name, stoppingToken)).QueueUrl;
    }

    private IRequest? TryGetDeserializedMessage(Message message)
    {
        try
        {
            var messageType = message.MessageAttributes["MessageType"].StringValue;
            var type = Type.GetType($"Sqs.Customers.Consumer.Contracts.{messageType}");
            if (type is null)
            {
                _logger.LogWarning("Unknown message type: {MessageType}", messageType);
                return null;
            }

            var typedMessage = (IRequest)JsonSerializer.Deserialize(message.Body, type)!;
            return typedMessage;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deserializing message");
            return null;
        }
    }
}