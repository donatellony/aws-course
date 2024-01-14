using Amazon.DynamoDBv2;
using Microsoft.Extensions.DependencyInjection;

namespace DynamoDb.Customers.DAL;

public static class DependencyInjection
{
    public static IServiceCollection AddDynamoDb(this IServiceCollection services)
    {
        services.AddSingleton<IAmazonDynamoDB, AmazonDynamoDBClient>();
        return services;
    }
}