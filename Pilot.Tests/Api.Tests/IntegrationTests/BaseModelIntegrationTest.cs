using Pilot.Identity.Data;
using Pilot.Identity.Models;
using Pilot.Tests.Api.Tests.IntegrationTests.Factories;
using Xunit;

namespace Pilot.Tests.Api.Tests.IntegrationTests;

public class BaseModelIntegrationTest : BaseApiIntegrationTest
{
    public BaseModelIntegrationTest(ApiTestApiFactory apiFactory, ApiTestReceiverFactory receiverFactory, ApiTestIdentityFactory identityFactory) : base(apiFactory, receiverFactory, identityFactory)
    {
    }

    [Fact]
    public void TestTest()
    {
        var user = new UserModel
        {
            Id = 0,
            UserName = "Test",
            Name = "Test",
            LastName = "Test",
            Password = "Test",
            Role = Role.User,
            Timestamp = DateTime.Now
        };

        IdentityContext.Add(user);
        IdentityContext.SaveChanges();
    }
}