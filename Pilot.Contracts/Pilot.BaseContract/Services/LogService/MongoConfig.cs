namespace Pilot.Contracts.Services.LogService;

public class MongoConfig
{
    public string ConnectionString { get; set; } 
    public string DbName { get; set; } 
    public string LogCollection { get; set; }

    public MongoConfig(string connectionString, string dbName, string logCollection)
    {
        ConnectionString = connectionString;
        DbName = dbName;
        LogCollection = logCollection;
    }
}