using MongoDB.Driver;
using Pilot.Contracts.Data;
using Pilot.Identity.Interfaces;
using Pilot.Identity.Models;

namespace Pilot.Identity.Data;

public class Seed : ISeed
{
    public async Task Seeding(IApplicationBuilder app)
    {
        var mongoDatabase = app.ApplicationServices.GetRequiredService<IMongoDatabase>();
        
        var userCollection = mongoDatabase.GetCollection<User>(MongoTable.User);

        var userExist = await userCollection.CountDocumentsAsync(FilterDefinition<User>.Empty);

        if (userExist != 0)
        {
            return;
        }
        
        var passwordCoder = app.ApplicationServices.GetRequiredService<IPasswordCoder>();
        
        var users = new List<User>()
        {
            new ()
            {
                UserName = "Admin",
                Name = "Yuri",
                LastName = "Bury",
                Password = passwordCoder.PasswordCode("123456"),
                Role = Role.Admin,
            },
            new ()
            {
                UserName = "Baget",
                Name = "Sasha",
                LastName = "Baginskiy",
                Password = passwordCoder.PasswordCode("123456"),
            },
            new ()
            {
                UserName = "JSCooler",
                Name = "Yarick",
                LastName = "Yanovich",
                Password = passwordCoder.PasswordCode("123456"),
                Role = Role.Admin,
            }
        };
        
        var mongoUserCollection = mongoDatabase.GetCollection<User>(MongoTable.User);
        await mongoUserCollection.InsertManyAsync(users);
    }
}