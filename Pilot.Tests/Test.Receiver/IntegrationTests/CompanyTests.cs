using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.RabbitMqMessages;
using Pilot.Receiver.Models;
using Test.Base.IntegrationBase;
using Test.Receiver.IntegrationTests.Factories;

namespace Test.Receiver.IntegrationTests;

public class CompanyTests : BaseModelReceiverIntegrationTest<Company, CompanyDto>
{
    /// <inheritdoc />
    public CompanyTests(ReceiverTestReceiverFactory receiverFactory, ReceiverTestIdentityFactory identityFactory) :
        base(receiverFactory, identityFactory)
    {
    }

    [Fact]
    public override async void CreateModel_ReturnOk()
    {
        #region Arrange

        var companyUser = await CreateCompanyUser();

        var value = GenerateTestEntity.CreateEntities<CompanyDto>(count: 1, listDepth: 0).First();
        value.CreateAt = DateTime.Now;
        value.Title = Guid.NewGuid().ToString();

        #endregion

        // Act

        await PublishEndpoint.Publish(new CreateCommandMessage<CompanyDto>(value, companyUser.Id));
        await Helper.Wait();

        // Assert

        var result = await ReceiverContext.Companies.Where(c => c.CreateAt == value.CreateAt).FirstOrDefaultAsync();

        Assert.NotNull(result);
        Assert.True(result.Title == value.Title);
    }
}