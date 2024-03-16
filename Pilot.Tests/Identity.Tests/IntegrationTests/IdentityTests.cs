using System.Net;
using System.Net.Http.Json;
using MongoDB.Driver;
using Pilot.Identity.Models;
using Pilot.Identity.Services;
using Pilot.Receiver.DTO;
using Xunit;

namespace Pilot.Tests.Identity.Tests.IntegrationTests;

public class IdentityTests : BaseIntegrationTest
{
    private readonly IMongoCollection<User> _collection;
    private readonly PasswordCoderService _coderService;
    
    public IdentityTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
        _coderService = new PasswordCoderService();
        _collection = MongoDatabase.GetCollection<User>("users");
    }
    
    
    [Fact]
    public async Task Registration_AddedUserToDb_ShouldReturnOK()
    {
        // Arrange
        var user = new RegistrationUserDto()
        {
            UserName = "Test",
            Name = "Test",
            LastName = "Test",
            Password = "Test"
        };
        
        // Act
        var request = await Client.PostAsJsonAsync("Registration", user);
        
        // Assert
        Assert.True(request.IsSuccessStatusCode);

        var filter = Builders<User>.Filter.Eq(x => x.UserName, user.UserName);
        
        var select = Builders<User>.Projection.Expression(x => new RegistrationUserDto
        {
            UserName = x.UserName,
            Name = x.Name,
            LastName = x.LastName,
            Password = x.Password
        });
        
        var userInDb = await _collection.Find(filter).Project(select).FirstOrDefaultAsync();
        
        Assert.Equal(user.UserName, userInDb.UserName);
        Assert.Equal(user.LastName, userInDb.LastName);
        Assert.Equal(user.Name, userInDb.Name);
        Assert.Equal(_coderService.PasswordCode(user.Password), userInDb.Password);
    }
    
    [Fact]
    public async Task Registration_UserAlreadyExist_ShouldReturnBadRequest()
    {
        // Arrange
        var userInDb = new User()
        {
            UserName = "Test",
            Name = "Test",
            LastName = "Test",
            Password = "Test"
        };
        
        await _collection.InsertOneAsync(userInDb);
        
        var user = new RegistrationUserDto()
        {
            UserName = "Test",
            Name = "Test",
            LastName = "Test",
            Password = "Test"
        };
        
        // Act
        var request = await Client.PostAsJsonAsync("Registration", user);
        
        // Assert
        Assert.False(request.IsSuccessStatusCode);
        Assert.True(request.StatusCode == HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Authorization_CheckUserExist_ShouldReturnOK()
    {
        // Arrange
        var pass = "Test";
        var user = new User()
        {
            UserName = $"Test+{Guid.NewGuid()}",
            Name = "Test",
            LastName = "Test",
            Password = _coderService.PasswordCode(pass)
        };

        var authUser = new AuthorizationUserDto
        {
            UserName = user.UserName,
            Password = pass,
        };

        await _collection.InsertOneAsync(user);
        
        // Act
        var request = await Client.PostAsJsonAsync("Authorization", authUser);
        
        // Assert
        Assert.True(request.IsSuccessStatusCode);
    }
    
    [Fact]
    public async Task Authorization_UserIsNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var authUser = new AuthorizationUserDto
        {
            UserName = "NotExist",
            Password = "Test",
        };

        // Act
        var request = await Client.PostAsJsonAsync("Authorization", authUser);
        
        // Assert
        Assert.True(!request.IsSuccessStatusCode);
        Assert.True(request.StatusCode == HttpStatusCode.NotFound);
    }
}
