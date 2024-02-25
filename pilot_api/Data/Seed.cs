using MongoDB.Driver;
using pilot_api.Models;

namespace pilot_api.Data;

public class Seed
{
    private readonly IMongoDatabase _mongoDatabase;
    
    public Seed(IMongoDatabase mongoDatabase)
    {
        _mongoDatabase = mongoDatabase;
    }

    public async Task Seeding()
    {
        // _mongoDatabase.GetCollections("users"); 
    }
}