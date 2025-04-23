using Microsoft.EntityFrameworkCore;
using Pilot.Capability.Models;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.RabbitMqMessages;
using Test.Base.IntegrationBase;
using Test.Capability.Factories;

namespace Test.Capability.IntegrationTests;

public class PostSkillTests(CapabilityTestCapabilityFactory factory)
    : BaseModelReceiverIntegrationTest<Post, PostDto>(factory)
{
    public override async Task UpdateModelTest_ReturnOk()
    {
        #region Arrange

        var value = GenerateTestEntity.CreateEntities<Post>(count: 1, listDepth: 0).First();

        var workerContext = AssertReceiverContext;
        await workerContext.AddAsync(value);
        value.Skills = [new Skill {Title = Guid.NewGuid().ToString()}, new Skill {Title = Guid.NewGuid().ToString()}];
        
        var skills = new List<Skill> {new(){Title = Guid.NewGuid().ToString()}, new(){Title = Guid.NewGuid().ToString()}};
        await workerContext.AddRangeAsync(skills);
        await workerContext.SaveChangesAsync();

        var valueDto = ReceiverMapper.Map<PostDto>(value);

        valueDto.Skills = [new SkillDto {Id = skills[0].Id}, new SkillDto {Id = skills[1].Id}];
        
        #endregion

        // Act

        await PublishEndpoint.Publish(new UpdateCommandMessage<PostDto>(valueDto, 1));
        await Helper.Wait();

        // Assert

        var result = await AssertReceiverContext.Set<Post>().Where(c => c.Id == value.Id).Include(c => c.Skills).FirstOrDefaultAsync();

        Assert.NotNull(result);
        Assert.NotNull(result.ChangeAt);
    }
}