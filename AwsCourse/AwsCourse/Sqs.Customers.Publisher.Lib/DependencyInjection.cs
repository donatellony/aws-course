using Amazon.SQS;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Sqs.Customers.Publisher.Lib.Messaging;
using Sqs.Customers.Publisher.Lib.Messaging.Settings;

namespace Sqs.Customers.Publisher.Lib;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddAmazonSns(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<QueueSettings>(builder.Configuration.GetSection(QueueSettings.Key));
        builder.Services.AddSingleton<IAmazonSQS, AmazonSQSClient>();
        builder.Services.AddSingleton<ISqsMessenger, SqsMessenger>();
        return builder;
    }
}