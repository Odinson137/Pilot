using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.DTO;
using Pilot.Identity.Models;
using Test.Identity.IntegrationTests.Factories;

namespace Test.Identity.IntegrationTests;

public class IdentityIntegrationTests : BaseIdentityIntegrationTest
{
    
    public IdentityIntegrationTests(IntegrationIdentityTestWebAppFactory factory)
        : base(factory) { }
    
    
    [Fact]
    public async Task Registration_AddedUserToDb_ShouldReturnOK()
    {
        // Arrange
        var user = new RegistrationUserDto
        {
            UserName = $"Test-{Guid.NewGuid()}",
            Name = "Test",
            LastName = "Test",
            Password = "Test"
        };

        // Act
        var request = await Client.PostAsJsonAsync("Registration", user);
        var _ = await request.Content.ReadAsStringAsync();
        
        // Assert
        Assert.True(request.IsSuccessStatusCode);
        
        var userInDb = await AssertContext.Users.Where(c => c.UserName == user.UserName).FirstOrDefaultAsync();
        
        Assert.NotNull(userInDb);

        Assert.Equal(user.UserName, userInDb.UserName);
        Assert.Equal(user.LastName, userInDb.LastName);
        Assert.Equal(user.Name, userInDb.Name);
        Assert.Equal(CoderService.ComparePasswordAndSalt(user.Password, userInDb.Salt), userInDb.Password);
    }
    
    [Fact]
    public async Task Registration_UserAlreadyExist_ShouldReturnBadRequest()
    {
        // Arrange
        var pas = "Test";
        var coder = CoderService.GenerateSaltAndHashPassword(pas);
        var userInDb = new User
        {
            UserName = "Test",
            Name = "Test",
            LastName = "Test",
            Password = coder.Item1,
            Salt = coder.Item2
        };
        
        await DataContext.AddAsync(userInDb);
        await DataContext.SaveChangesAsync();
        
        var user = new RegistrationUserDto
        {
            UserName = "Test",
            Name = "Test",
            LastName = "Test",
            Password = pas
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
        var pas = "Test";
        var coder = CoderService.GenerateSaltAndHashPassword(pas);
        var user = new User
        {
            UserName = $"Test+{Guid.NewGuid()}",
            Name = "Test",
            LastName = "Test",
            Password = coder.Item1,
            Salt = coder.Item2
        };

        var authUser = new AuthorizationUserDto
        {
            UserName = user.UserName,
            Password = pas
        };

        await DataContext.AddAsync(user);
        await DataContext.SaveChangesAsync();
        
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

