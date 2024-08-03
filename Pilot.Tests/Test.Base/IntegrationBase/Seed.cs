using Microsoft.AspNetCore.Builder;
using Pilot.Contracts.Data;

namespace Test.Base.IntegrationBase;

public class TestSeed : ISeed
{
    public Task Seeding(IApplicationBuilder app)
    {
        return Task.CompletedTask;
    }
}