using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Identity.Models;
using Pilot.Worker.Models;
using Pilot.Worker.Models.ModelHelpers;
using Test.Api.IntegrationTests.Factories;
using Test.Base.IntegrationBase;

namespace Test.Api.IntegrationTests;

[Collection(nameof(SequentialCollectionDefinition))]
public abstract class BaseModelIntegrationTest<T, TDto>(
    ApiTestApiFactory apiFactory,
    ApiTestWorkerFactory workerFactory,
    ApiTestIdentityFactory identityFactory) : BaseApiIntegrationTest(apiFactory, workerFactory, identityFactory)
    where T : BaseModel where TDto : BaseDto
{
    // // ReSharper disable once MemberCanBePrivate.Global
    // protected async Task<CompanyUser> CreateCompanyUser(bool withAuthorization = false)
    // {
    //     var companyUser = GenerateTestEntity.CreateEntities<CompanyUser>(count: 1, listDepth: 0).First();
    //
    //     await GetContext<TDto>().AddAsync(companyUser);
    //     await GetContext<TDto>().SaveChangesAsync();
    //
    //     var user = GenerateTestEntity.CreateEntities<User>(count: 1).First();
    //     user.Id = companyUser.Id;
    //
    //     await GetContext<UserDto>().AddRangeAsync(user);
    //     await GetContext<UserDto>().SaveChangesAsync();
    //
    //     if (withAuthorization)
    //     {
    //         var token = TokenService.GenerateToken(companyUser.Id, Role.User);
    //         ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    //     }
    //     
    //     return companyUser;
    // }
    //
    // public BaseModelIntegrationTest(ApiTestApiFactory apiFactory, ApiTestWorkerFactory workerFactory,
    //     ApiTestIdentityFactory identityFactory, ApiTestCapabilityFactory capabilityFactory) : base(apiFactory, workerFactory, identityFactory) {}
    //
    // [Fact]
    // public async Task GetAllValuesTest_ReturnOk()
    // {
    //     #region Arrange
    //
    //     var type = typeof(T);
    //     const int count = 2;
    //
    //     var values = GenerateTestEntity.CreateEntities(type, count: count, listDepth: 0);
    //
    //     await GetContext<TDto>().AddRangeAsync(values);
    //     await GetContext<TDto>().SaveChangesAsync();
    //
    //     #endregion 
    //
    //     // Act
    //     var result = await ApiClient.GetAsync($"api/{type.Name}");
    //
    //     // Assert
    //     Assert.True(result.IsSuccessStatusCode);
    //     var content = await result.Content.ReadFromJsonAsync<ICollection<BaseDto>>();
    //     Assert.NotNull(content);
    //     Assert.True(content.Count >= count);
    // }
    //
    // [Fact]
    // public async Task GetValue_ReturnOk()
    // {
    //     #region Arrange
    //
    //     var type = typeof(T);
    //     const int count = 2;
    //
    //     var values = GenerateTestEntity.CreateEntities(type, count: count, listDepth: 0);
    //
    //     await GetContext<TDto>().AddRangeAsync(values);
    //     await GetContext<TDto>().SaveChangesAsync();
    //
    //     var id = values.First().Id;
    //
    //     #endregion
    //
    //     // Act
    //     var result = await ApiClient.GetAsync($"api/{type.Name}/{id}");
    //
    //     // Assert
    //     Assert.True(result.IsSuccessStatusCode);
    //     var content = await result.Content.ReadFromJsonAsync<BaseDto>();
    //     Assert.NotNull(content);
    //     Assert.Equal(id, content.Id);
    // }
    //
    // [Fact]
    // public async Task CreateValue_ReturnOk()
    // {
    //     #region Arrange
    //
    //     await CreateCompanyUser(true);
    //     
    //     var type = typeof(T);
    //
    //     var value = GenerateTestEntity.CreateEntities<T>(count: 1, listDepth: 0).First();
    //
    //     await GenerateTestEntity.FillChildren(value, GetContext<TDto>());
    //     
    //     var valueDto = ReceiverMapper.Map<TDto>(value);
    //     
    //     #endregion
    //
    //     // Act
    //     var result = await ApiClient.PostAsJsonAsync($"api/{type.Name}", valueDto);
    //     await Helper.Wait();
    //
    //     // Assert
    //     Assert.True(result.IsSuccessStatusCode);
    //     var content = await AssertReceiverContext.Set<T>().Where(c => c.CreateAt == value.CreateAt).FirstOrDefaultAsync();
    //     Assert.NotNull(content);
    // }
    //     
    // [Fact]
    // public async Task UpdateValue_ReturnOk()
    // {
    //     #region Arrange
    //
    //     var companyUser = await CreateCompanyUser(true);
    //
    //     var type = typeof(T);
    //
    //     var value = GenerateTestEntity.CreateEntities<T>(count: 1, listDepth: 0).First();
    //
    //     if (value is IAddCompanyUser addCompanyUser) addCompanyUser.AddCompanyUser(companyUser);
    //
    //     await GetContext<TDto>().AddRangeAsync(value);
    //     await GetContext<TDto>().SaveChangesAsync();
    //
    //     var valueDto = ReceiverMapper.Map<TDto>(value);
    //     
    //     #endregion
    //
    //     // Act
    //     var result = await ApiClient.PutAsJsonAsync($"api/{type.Name}", valueDto);
    //     await Helper.Wait();
    //
    //     // Assert
    //     Assert.True(result.IsSuccessStatusCode);
    //     var content = await AssertReceiverContext.Set<T>().Where(c => c.Id == value.Id).FirstOrDefaultAsync();
    //     Assert.NotNull(content);
    //     Assert.NotNull(content.ChangeAt);
    // }
    //
    // [Fact]
    // public async Task DeleteValue_ReturnOk()
    // {
    //     #region Arrange
    //
    //     await CreateCompanyUser(true);
    //
    //     var type = typeof(T);
    //
    //     var value = GenerateTestEntity.CreateEntities<T>(count: 1, listDepth: 0).First();
    //
    //     await GetContext<TDto>().AddRangeAsync(value);
    //     await GetContext<TDto>().SaveChangesAsync();
    //     
    //     #endregion
    //
    //     // Act
    //     var result = await ApiClient.DeleteAsync($"api/{type.Name}/{value.Id}");
    //     await Helper.Wait();
    //
    //     // Assert
    //     
    //     Assert.True(result.IsSuccessStatusCode);
    //     var content = await AssertReceiverContext.Set<T>().Where(c => c.Id == value.Id).FirstOrDefaultAsync();
    //     Assert.Null(content);
    // }
}