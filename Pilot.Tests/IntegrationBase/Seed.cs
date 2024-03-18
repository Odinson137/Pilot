using Microsoft.AspNetCore.Builder;
using Pilot.Contracts.Data;

namespace Pilot.Tests.IntegrationBase;

public class TestSeed : ISeed
{
    public Task Seeding(IApplicationBuilder app)
    {
        return Task.CompletedTask;
    }
}