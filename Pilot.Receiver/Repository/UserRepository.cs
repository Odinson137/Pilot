using MongoDB.Driver;
using Pilot.Contracts.Data;
using Pilot.Contracts.Models;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Repository;

public class UserRepository : IUser
{
    private readonly IMongoCollection<User> _mongoCollection;

    public UserRepository(IMongoDatabase mongo)
    {
        _mongoCollection = mongo.GetCollection<User>(MongoTable.User);
    }

    public async Task<User?> GetUserByIdAsync(string userId)
    {
        var filter = Builders<User>.Filter.Eq(c => c.Id, userId);
        var user = await _mongoCollection.Find(filter).FirstOrDefaultAsync();
        return user;
    }
}