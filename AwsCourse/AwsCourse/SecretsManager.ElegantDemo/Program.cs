var builder = WebApplication.CreateBuilder(args);

var env = builder.Environment.EnvironmentName;
var appName = builder.Environment.ApplicationName;

// Secret name in the Secret Manager should look like:
// Development_SecretsManager.ElegantDemo_OuterConfigSection__InnerConfigSection__InnerOfTheInnerConfigSection
builder.Configuration.AddSecretsManager(configurator: options =>
{
    options.SecretFilter = entry => entry.Name.StartsWith($"{env}_{appName}");
    options.KeyGenerator = (_, s) => s
        .Replace($"{env}_{appName}_", string.Empty)
        .Replace("__", ";");
    options.PollingInterval = TimeSpan.FromHours(1); // Try to refresh the configuration automatically
});

// Polling interval requires the usage of IOptionsMonitor<OuterConfigSection> instead of IOptions<OuterConfigSection>
// Interval should not be short, as you're charged per every request to the Secrets Manager :)