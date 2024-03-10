using MongoDB.Driver;
using Pilot.Identity.Data;
using Pilot.Identity.Interfaces;
using Pilot.Identity.Models;

namespace Pilot.Identity.Repository;

public class UserRepository : IUser
{
    private readonly IMongoCollection<User> _userCollection;

    public UserRepository(IMongoDatabase mongo)
    {
        _userCollection = mongo.GetCollection<User>(MongoTable.User);
    }

    public async Task RegistrationAsync(User user)
    {
        await _userCollection.InsertOneAsync(user);
    }

    public async Task<bool> IsUserNameExistAsync(string userName)
    {
        var filter = Builders<User>.Filter.Eq(c => c.UserName, userName);
        var isExist = await _userCollection.Find(filter).AnyAsync();
        return isExist;
    }

    public async Task<User?> GetUserAsync(string userId)
    {
        var filter = Builders<User>.Filter.Eq(c => c.Id, userId);
        var user = await _userCollection.Find(filter).FirstOrDefaultAsync();
        return user;
    }
}