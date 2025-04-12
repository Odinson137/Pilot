using System.Linq.Expressions;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Services;
using Pilot.Identity.Models;
using Pilot.SqrsControllerLibrary.RabbitMqMessages;
using Serialize.Linq.Serializers;
using Test.Base.IntegrationBase;
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
        
        var userInDb = await AssertReceiverContext.Users.Where(c => c.UserName == user.UserName).FirstOrDefaultAsync();
        
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

        [Fact]
    public virtual async Task GetAllValuesTest_FilterWithIds_ReturnOk()
    {
        #region Arrange

        const int count = 3;
        var values = GenerateTestEntity.CreateEntities<User>(count: count, listDepth: 0);

        await DataContext.AddRangeAsync(values);
        await DataContext.SaveChangesAsync();

        var filter = new BaseFilter
        {
            Ids = values.Select(c => c.Id).ToList(),
        };

        #endregion

        // Act
        var result = await Client.GetAsync($"api/{nameof(User)}?filter={filter.ToJson()}");

        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var content = await result.Content.ReadFromJsonAsync<ICollection<UserDto>>();
        Assert.NotNull(content);
        Assert.True(content.Count >= count);
    }

    [Fact]
    public virtual async Task GetAllValuesTest_FilterWithWhereFilter_ReturnOk()
    {
        #region Arrange

        const int count = 3;
        var values = GenerateTestEntity.CreateEntities<User>(count: count, listDepth: 0);

        await DataContext.AddRangeAsync(values);
        await DataContext.SaveChangesAsync();

        var filter = new BaseFilter
        {
            WhereFilter = new WhereFilter((nameof(BaseId.Id), values.Select(c => c.Id).First()))
        };

        #endregion

        // Act
        var result = await Client.GetAsync($"api/{nameof(User)}?filter={filter.ToJson()}");

        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var content = await result.Content.ReadFromJsonAsync<ICollection<UserDto>>();
        Assert.NotNull(content);
        Assert.True(content.Count == 1);
    }

    [Fact]
    public virtual async Task GetAllValuesTest_WithSelectQuery_ReturnOk()
    {
        #region Arrange

        const int count = 3;
        var values = GenerateTestEntity.CreateEntities<User>(count: count, listDepth: 0);

        await DataContext.AddRangeAsync(values);
        await DataContext.SaveChangesAsync();

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

    [Fact]
    public virtual async Task GetAllValuesTest_ReturnOk()
    {
        #region Arrange

        const int count = 2;
        var values = GenerateTestEntity.CreateEntities<User>(count: count, listDepth: 0);

        await DataContext.AddRangeAsync(values);
        await DataContext.SaveChangesAsync();

        #endregion

        // Act
        var result = await Client.GetAsync($"api/{nameof(User)}");

        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var content = await result.Content.ReadFromJsonAsync<ICollection<UserDto>>();
        Assert.NotNull(content);
        Assert.True(content.Count >= count);
    }

    [Fact]
    public virtual async Task GetValue_ReturnOk()
    {
        #region Arrange

        const int count = 1;

        var values = GenerateTestEntity.CreateEntities<User>(count: count, listDepth: 0);

        await DataContext.AddRangeAsync(values);
        await DataContext.SaveChangesAsync();

        var id = values.First().Id;

        #endregion

        // Act
        var result = await Client.GetAsync($"api/{nameof(User)}/{id}");

        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var content = await result.Content.ReadFromJsonAsync<UserDto>();
        Assert.NotNull(content);
        Assert.Equal(id, content.Id);
    }

    [Fact]
    public virtual async Task UpdateModelTest_ReturnOk()
    {
        #region Arrange

        var user = await CreateUser();
        var value = GenerateTestEntity.CreateEntities<User>(count: 1, listDepth: 0).First();

        await DataContext.AddAsync(value);
        await DataContext.SaveChangesAsync();

        var valueDto = Mapper.Map<UserDto>(value);

        #endregion

        // Act

        await PublishEndpoint.Publish(new UpdateCommandMessage<UserDto>(valueDto, user.Id));
        await Helper.Wait();

        // Assert

        var result = await AssertReceiverContext.Set<User>().Where(c => c.Id == value.Id).FirstOrDefaultAsync();

        Assert.NotNull(result);
        Assert.NotNull(result.ChangeAt);
    }
    
    [Fact]
    public virtual async Task DeleteModelTest_ReturnOk()
    {
        #region Arrange
    
        var user = await CreateUser();
    
        var value = GenerateTestEntity.CreateEntities<User>(count: 1, listDepth: 0).First();
    
        await DataContext.AddAsync(value);
        await DataContext.SaveChangesAsync();
    
        #endregion
    
        // Act
    
        await PublishEndpoint.Publish(new DeleteCommandMessage<UserDto>(value.Id, user.Id));
        await Helper.Wait();
    
        // Assert
    
        var result = await AssertReceiverContext.Set<User>().Where(c => c.Id == value.Id).FirstOrDefaultAsync();
    
        Assert.Null(result);
    }

    protected async Task<User> CreateUser()
    {
        var companyUser = GenerateTestEntity.CreateEntities<User>(count: 1, listDepth: 0).First();

        await DataContext.AddAsync(companyUser);
        await DataContext.SaveChangesAsync();

        return companyUser;
    }

}

