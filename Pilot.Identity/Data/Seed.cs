using MongoDB.Driver;
using Pilot.Identity.Models;

namespace Pilot.Identity.Data;

public static class Seed
{
    public static async Task Seeding(this IApplicationBuilder app)
    {
        var mongoDatabase = app.ApplicationServices.GetRequiredService<IMongoDatabase>();
        
        var userCollection = mongoDatabase.GetCollection<User>(MongoTable.User);

        var userExist = await userCollection.CountDocumentsAsync(FilterDefinition<User>.Empty);

        if (userExist != 0)
        {
            return;
        }
        
        var users = new List<User>()
        {
            new ()
            {
                UserName = "Admin",
                Name = "Yuri",
                LastName = "Bury",
                Password = "123456",
                Role = Role.Admin,
            },
            new ()
            {
                UserName = "Baget",
                Name = "Sasha",
                LastName = "Baginskiy",
                Password = "123456",
            },
            new ()
            {
                UserName = "JSCooler",
                Name = "Yarick",
                LastName = "Yanovich",
                Password = "123456",
                Role = Role.Admin,
            }
        };
        
        var mongoUserCollection = mongoDatabase.GetCollection<User>(MongoTable.User);
        await mongoUserCollection.InsertManyAsync(users);
    }
}