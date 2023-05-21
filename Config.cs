namespace Shared;

using Microsoft.Extensions.Configuration;

public class Config
{
    public string PrimaryRedisConnectionString { get; }
    public string SecondaryRedisConnectionString { get; }

    public Config()
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        PrimaryRedisConnectionString = config[$"redis:{config["redis:primary"]}:connectionString"] ?? "<EMPTY>";
        SecondaryRedisConnectionString = config[$"redis:{config["redis:secondary"]}:connectionString"] ?? "<EMPTY>";
    }

    public override string ToString()
    {
        return $"PrimaryRedisConnectionString: {PrimaryRedisConnectionString}\n" +
            $"SecondaryRedisConnectionString: {SecondaryRedisConnectionString}";
    }
}
