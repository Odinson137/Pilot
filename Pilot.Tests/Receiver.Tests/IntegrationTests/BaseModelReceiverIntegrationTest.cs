using System.Net.Http.Json;
using System.Reflection;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Models;
using Pilot.Contracts.RabbitMqMessages;
using Pilot.Identity.Models;
using Pilot.Tests.IntegrationBase;
using Pilot.Tests.Receiver.Tests.IntegrationTests.Factories;
using Xunit;

namespace Pilot.Tests.Receiver.Tests.IntegrationTests;

public class BaseModelReceiverIntegrationTest : BaseReceiverIntegrationTest
{
    private readonly User _admin;
    
    public BaseModelReceiverIntegrationTest(ReceiverTestReceiverFactory receiverFactory, ReceiverTestIdentityFactory identityFactory) : base(receiverFactory, identityFactory)
    {
        _admin = new User
        {
            UserName = "Admin",
            Name = "AdminName",
            LastName = "AdminLastName",
            Password = "12345678",
            Role = Role.Admin,
        };

        IdentityContext.Add(_admin);
        IdentityContext.SaveChanges();
    }

    public static IEnumerable<object[]> ModelData
    {
        get
        {
            var baseModelType = typeof(BaseModel);
            var assembly = Assembly.GetAssembly(baseModelType);

            var modelTypes = assembly?.GetTypes()
                .Where(t => t is { IsClass: true, IsAbstract: false } && t.IsSubclassOf(baseModelType))
                .Select(c => new object[] { c })
                .ToList();

            return modelTypes!;
        }
    }
    
    // Маленькое ухищрение, которое позволяет ожидать пока консюмер обработает сообщение из очереди,
    // А ещё нормально использовать debug, имея в запасе 20 кликов в нём
    private async Task Wait()
    {
        for (var i = 0; i < 20; i++) await Task.Delay(200);
    }
    
    [Theory]
    [TestBeforeAfter]
    [MemberData(nameof(ModelData))]
    public async void GetAllValuesTest_ReturnOk(Type type, int count = 2)
    {
        #region Arrange

        var values = GenerateTestEntity.CreateEntities(type, count: count, listDepth: 0);
        
        await ReceiverContext.AddRangeAsync(values);
        await ReceiverContext.SaveChangesAsync();
        
        #endregion

        // Act
        var result = await ReceiverClient.GetAsync($"api/{type.Name}");
        
        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var content = await result.Content.ReadFromJsonAsync<ICollection<BaseDto>>();
        Assert.NotNull(content);
        Assert.True(content.Count >= count);
    }
    
    [Theory]
    [TestBeforeAfter]
    [MemberData(nameof(ModelData))]
    public async void GetValue_ReturnOk(Type type, int count = 1)
    {
        #region Arrange

        var values = GenerateTestEntity.CreateEntities(type, count: count, listDepth: 0);
        
        await ReceiverContext.AddRangeAsync(values);
        await ReceiverContext.SaveChangesAsync();

        var id = values.First().Id;
        
        #endregion

        // Act
        var result = await ReceiverClient.GetAsync($"api/{type.Name}/{id}");
        
        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var content = await result.Content.ReadFromJsonAsync<BaseDto>();
        Assert.NotNull(content);
        Assert.Equal(id, content.Id);
    }
    
    [Fact]
    [TestBeforeAfter]
    public async void CreateModel_ReturnOk()
    {
        #region Arrange

        var user = GenerateTestEntity.CreateEntities<User>(count: 1).First();

        await IdentityContext.AddRangeAsync(user);
        await IdentityContext.SaveChangesAsync();
        
        var value = GenerateTestEntity.CreateEntities<CompanyDto>(count: 1).First();
        value.Title = Guid.NewGuid().ToString();
        
        var companyUser = GenerateTestEntity.CreateDtEntities<CompanyUser>(count: 1).First();
        companyUser.Id = user.Id;
        
        await ReceiverContext.AddAsync(companyUser);
        await ReceiverContext.SaveChangesAsync();
        
        #endregion

        // Act

        await PublishEndpoint.Publish(new CreateCommandMessage<CompanyDto>(value, _admin.Id));

        // Assert
        await Wait();

        var result = await ReceiverContext.Companies.Where(c => c.Title == value.Title).FirstOrDefaultAsync();

        Assert.NotNull(result);
        Assert.True(result.Title == value.Title);
    }
    
    [Fact]
    [TestBeforeAfter]
    public async void UpdateModelTest_ReturnOk()
    {
        #region Arrange

        var user = GenerateTestEntity.CreateEntities<User>(count: 1).First();

        await IdentityContext.AddRangeAsync(user);
        await IdentityContext.SaveChangesAsync();
        
        var value = GenerateTestEntity.CreateEntities<Company>(count: 1).First();
        
        var companyUser = GenerateTestEntity.CreateEntities<CompanyUser>(count: 1).First();
        companyUser.Id = user.Id;
        
        value.CompanyUsers = new List<CompanyUser> {companyUser};

        await ReceiverContext.AddAsync(value);
        await ReceiverContext.SaveChangesAsync();

        var mapper = ReceiverScope.ServiceProvider.GetRequiredService<IMapper>();
        var valueDto = mapper.Map<CompanyDto>(value);
        
        // Как-то редактируем модель
        valueDto.Title = Guid.NewGuid().ToString();
        
        #endregion

        // Act

        await PublishEndpoint.Publish(new UpdateCommandMessage<CompanyDto>(valueDto, _admin.Id));

        // Assert
        await Wait();
        
        var result = await AssertReceiverContext.Companies.Where(c => c.Id == value.Id).FirstOrDefaultAsync();

        Assert.NotNull(result);
        Assert.True(result.Title == valueDto.Title);
    }
}