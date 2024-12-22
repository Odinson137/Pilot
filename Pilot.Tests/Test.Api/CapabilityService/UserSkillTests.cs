using System.Net.Http.Json;
using Pilot.Capability.Models;
using Pilot.Contracts.Data;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Messenger.Models;
using Test.Api.CapabilityService.Factory;
using Test.Base.IntegrationBase;

namespace Test.Api.CapabilityService;

public class UserSkillTests : CapabilityTests<UserSkill, UserSkillDto>
{
    /// <inheritdoc />
    public UserSkillTests(
        CapabilityTestApiFactory apiFactory, 
        CapabilityTestIdentityFactory identityFactory, 
        CapabilityTestCapabilityFactory capabilityFactory, 
        CapabilityTestStorageFactory storageFactory)
        : base(apiFactory, identityFactory, capabilityFactory, storageFactory)
    {
    }
    
    [Fact]
    public virtual async Task GetUserSkillTest_ReturnOk()
    {
        #region Arrange
    
        const int count = 1;
        var skill = GenerateTestEntity.CreateEntities<Skill>(count: count, listDepth: 0).First();

        await GetContext<SkillDto>().AddRangeAsync(skill);
        
        var userSkill = GenerateTestEntity.CreateEntities<UserSkill>(count: count, listDepth: 0).First();
        
        await GetContext<SkillDto>().AddRangeAsync(userSkill);

        userSkill.Skill = skill;
        userSkill.UserId = 1;
        
        await GetContext<SkillDto>().SaveChangesAsync();
        
        #endregion
    
        // Act
        var result = await ApiClient.GetAsync($"api/{EntityName}/{Urls.UserSkills}/{userSkill.UserId}");
    
        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var content = await result.Content.ReadFromJsonAsync<ICollection<SkillDto>>();
        Assert.NotNull(content);
        Assert.True(content.Count >= count);
    }
}