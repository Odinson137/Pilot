using System.Linq.Expressions;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Services;
using Pilot.Identity.Data;
using Pilot.Identity.Interfaces;
using Pilot.Identity.Models;
using Serialize.Linq.Serializers;
using Test.Base.IntegrationBase;
using Test.Base.IntegrationBase.Factories;
using Xunit.Abstractions;

namespace Test.Identity.IntegrationTests;

public class IdentityIntegrationTests(
    TestIdentityFactory identityFactory,
    ITestOutputHelper testOutputHelper)
    : BaseServiceModelTests<User, UserDto>(testOutputHelper, ServiceName.IdentityServer,
            configurations:
            [
                new ServiceTestConfiguration
                {
                    ServiceName = ServiceName.IdentityServer,
                    ServiceProvider = identityFactory.Services,
                    DbContextType = typeof(DataContext),
                    HttpClient = identityFactory.CreateClient(),
                    IsMainService = true
                }
            ]),
        IClassFixture<TestIdentityFactory>
{
    private IPasswordCoder CoderService => identityFactory.Services.GetRequiredService<IPasswordCoder>();

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

        var userInDb = await ((DataContext)GetContext(ServiceName.IdentityServer)).Users
            .Where(c => c.UserName == user.UserName).FirstOrDefaultAsync();

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

        var dataContext = (DataContext)GetContext(ServiceName.IdentityServer);
        await dataContext.AddAsync(userInDb);
        await dataContext.SaveChangesAsync();

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

        var dataContext = (DataContext)GetContext(ServiceName.IdentityServer);
        await dataContext.AddAsync(user);
        await dataContext.SaveChangesAsync();

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

    [Fact]
    public virtual async Task GetAllValuesTest_WithSelectQuery_ReturnOk()
    {
        #region Arrange

        const int count = 3;
        var values = GenerateTestEntity.CreateEntities<User>(count: count, listDepth: 0);

        var dataContext = (DataContext)GetContext(ServiceName.IdentityServer);
        await dataContext.AddRangeAsync(values);
        await dataContext.SaveChangesAsync();

        Expression<Func<User, object>> projection = c => new { c.Id };
        var filter = new BaseFilter
        {
            SelectQuery = new ExpressionSerializer(new JsonSerializer()).SerializeText(projection)
        };

        #endregion

        // Act
        var content = new StringContent(filter.ToJson(), Encoding.UTF8, "application/json");
        var result = await Client.PostAsync($"api/{nameof(User)}/Query", content);

        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var resultContent = await result.Content.ReadFromJsonAsync<ICollection<BaseDto>>();
        Assert.NotNull(resultContent);
        Assert.True(resultContent.Count >= count);
    }

    [Fact(Skip = "Не нужен. Создание происходит через процесс регистрации")]
    public override Task CreateModelTest_ReturnOk()
    {
        return Task.CompletedTask;
    }
}