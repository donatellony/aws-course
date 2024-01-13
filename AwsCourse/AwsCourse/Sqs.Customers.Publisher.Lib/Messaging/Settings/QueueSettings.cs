namespace Sqs.Customers.Publisher.Lib.Messaging.Settings;

public class QueueSettings
{
    public const string Key = "Queue"; // Key name in appsettings.json
    public required string Name { get; init; }
}