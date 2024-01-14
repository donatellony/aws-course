namespace Sns.Customers.Publisher.Lib.Messaging.Settings;

public class TopicSettings
{
    public const string Key = "Topic"; // Key name in appsettings.json
    public required string Name { get; init; }
}