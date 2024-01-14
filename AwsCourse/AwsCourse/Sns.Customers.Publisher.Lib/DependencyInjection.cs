using Amazon.SimpleNotificationService;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Sns.Customers.Publisher.Lib.Messaging;
using Sns.Customers.Publisher.Lib.Messaging.Settings;

namespace Sns.Customers.Publisher.Lib;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddAmazonSns(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<TopicSettings>(builder.Configuration.GetSection(TopicSettings.Key));
        builder.Services.AddSingleton<IAmazonSimpleNotificationService, AmazonSimpleNotificationServiceClient>();
        builder.Services.AddSingleton<ISnsMessenger, SnsMessenger>();
        return builder;
    }
}