using Microsoft.Extensions.Configuration;

public class TestConfigurationProvider : ConfigurationProvider
{
    private readonly Dictionary<string, string> _configOverrides;

    public TestConfigurationProvider(Dictionary<string, string> configOverrides)
    {
        _configOverrides = configOverrides;
    }

    public override void Load()
    {
        foreach (var configOverride in _configOverrides)
        {
            Data[configOverride.Key] = configOverride.Value;
        }
    }
}