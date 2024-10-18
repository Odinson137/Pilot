using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Identity.Models;
using Test.Api.IntegrationTests.Factories;

namespace Test.Api.UserService;

public class UserTests : BaseUserServiceIntegrationTest
{
    public UserTests(ApiTestApiFactory apiFactory, ApiTestIdentityFactory identityFactory)
        : base(apiFactory, identityFactory) { }

    [Fact]
    public async Task Registration_AddedUserToDb_ShouldReturnOK()
    {
        // Arrange
        var user = new RegistrationUserDto
        {
            UserName = $"Test-{Guid.NewGuid()}",
            Name = "Test",
            LastName = "Test",
            Password = "TestPassword123"
        };

        // Act
        var response = await ApiClient.PostAsJsonAsync("/api/User/Registration", user);
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.True(response.IsSuccessStatusCode);

        var userInDb = await AssertWorkerContext.Users
            .Where(u => u.UserName == user.UserName)
            .FirstOrDefaultAsync();

        Assert.NotNull(userInDb);
        Assert.Equal(user.UserName, userInDb.UserName);
        Assert.Equal(user.LastName, userInDb.LastName);
        Assert.Equal(user.Name, userInDb.Name);
        Assert.NotEmpty(PasswordCoder.ComparePasswordAndSalt(user.Password, userInDb.Salt));
    }
    //
    // [Fact]
    // public async Task Registration_UserAlreadyExist_ShouldReturnBadRequest()
    // {
    //     // Arrange
    //     var password = "TestPassword123";
    //     var coder = PasswordCoder.GenerateSaltAndHashPassword(password);
    //
    //     var existingUser = new User
    //     {
    //         UserName = "ExistingUser",
    //         Name = "Test",
    //         LastName = "User",
    //         Password = coder.Item1,
    //         Salt = coder.Item2
    //     };
    //
    //     await GetContext<UserDto>().AddAsync(existingUser);
    //     await GetContext<UserDto>().SaveChangesAsync();
    //
    //     var user = new RegistrationUserDto
    //     {
    //         UserName = "ExistingUser",
    //         Name = "Test",
    //         LastName = "User",
    //         Password = password
    //     };
    //
    //     // Act
    //     var response = await ApiClient.PostAsJsonAsync("/api/User/Registration", user);
    //
    //     // Assert
    //     Assert.False(response.IsSuccessStatusCode);
    //     Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    // }

    [Fact]
    public async Task Authorization_CheckUserExist_ShouldReturnOK()
    {
        // Arrange
        var password = "TestPassword123";
        var coder = PasswordCoder.GenerateSaltAndHashPassword(password);

        var existingUser = new User
        {
            UserName = $"Test-{Guid.NewGuid()}",
            Name = "Test",
            LastName = "User",
            Password = coder.Item1,
            Salt = coder.Item2
        };

        await GetContext<UserDto>().AddAsync(existingUser);
        await GetContext<UserDto>().SaveChangesAsync();

        var authUser = new AuthorizationUserDto
        {
            UserName = existingUser.UserName,
            Password = password
        };

        // Act
        var response = await ApiClient.PostAsJsonAsync("/api/User/Authorization", authUser);

        // Assert
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task Authorization_UserIsNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var authUser = new AuthorizationUserDto
        {
            UserName = $"NonExistentUser-{Guid.NewGuid()}",
            Password = "TestPassword123"
        };

        // Act
        var response = await ApiClient.PostAsJsonAsync("/api/User/Authorization", authUser);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
    }
    
    [Fact]
    public async Task GetUser_ShouldReturnOk()
    {
        // Arrange
        var password = "TestPassword123";
        var coder = PasswordCoder.GenerateSaltAndHashPassword(password);

        var existingUser = new User
        {
            UserName = $"Test-{Guid.NewGuid()}",
            Name = "Test",
            LastName = "User",
            Password = coder.Item1,
            Salt = coder.Item2
        };

        await GetContext<UserDto>().AddAsync(existingUser);
        await GetContext<UserDto>().SaveChangesAsync();

        var token = TokenService.GenerateToken(existingUser.Id, Role.User);
        ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        // Act
        var response = await ApiClient.GetAsync("/api/User");

        // Assert
        Assert.True(response.IsSuccessStatusCode);
    }
}