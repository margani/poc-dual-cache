namespace PoCDualCache;

using System;
using Shared;
using StackExchange.Redis;

class Program
{
    static void Main(string[] args)
    {
        var config = new Config();
        var dualCache = new DualCache(config.PrimaryRedisConnectionString, config.SecondaryRedisConnectionString);

        dualCache.TestPutGet();
    }
}

class DualCache
{
    private const int DefaultDatabaseNumber = 0;
    private ConnectionMultiplexer PrimaryConnectionMultiplexer { get; }
    private ConnectionMultiplexer SecondaryConnectionMultiplexer { get; }

    private IDatabase PrimaryDatabase { get; }
    private IDatabase SecondaryDatabase { get; }

    public DualCache(string primaryConnectionString, string secondaryConnectionString)
    {
        PrimaryConnectionMultiplexer = ConnectionMultiplexer.Connect(primaryConnectionString);
        PrimaryDatabase = PrimaryConnectionMultiplexer.GetDatabase(DefaultDatabaseNumber);

        SecondaryConnectionMultiplexer = ConnectionMultiplexer.Connect(secondaryConnectionString);
        SecondaryDatabase = SecondaryConnectionMultiplexer.GetDatabase(DefaultDatabaseNumber);
    }

    private void Put(string key, string value)
    {
        PrimaryDatabase.StringSet(key, value);
        SecondaryDatabase.StringSet(key, value);
    }

    private string Get(string key)
    {
        var valueInPrimary = PrimaryDatabase.StringGet(key).ToString();
        var valueInSecondary = SecondaryDatabase.StringGet(key).ToString();
        if (valueInPrimary != valueInSecondary)
        {
            Console.WriteLine($"[Warning] Value [{valueInPrimary}] in Primary redis is different " +
                "from value[{valueInSecondary}] in Secondary redis");
        }

        return valueInPrimary;
    }

    public void TestPutGet()
    {
        var key = "key for dual cache poc";
        Put(key, "example value");
        var value = Get(key);
        Console.WriteLine($"Returned value: {value}");
    }
}
