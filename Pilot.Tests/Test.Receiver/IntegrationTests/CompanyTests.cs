using Pilot.Contracts.DTO.ModelDto;
using Pilot.Receiver.Models;
using Test.Receiver.IntegrationTests.Factories;

namespace Test.Receiver.IntegrationTests;

public class CompanyTests : BaseModelReceiverIntegrationTest<Company, CompanyDto>
{
    /// <inheritdoc />
    public CompanyTests(ReceiverTestReceiverFactory receiverFactory, ReceiverTestIdentityFactory identityFactory) :
        base(receiverFactory, identityFactory)
    {
    }
    
    protected override async ValueTask GetArrangeDop(ICollection<Company> values)
    {
        foreach (var company in values)
        {
            company.LogoId = 1;
            company.InsideImagesId = [1];
        }
    }
}