using Pilot.Contracts.Data;

namespace Test.Base.IntegrationBase;

public class TestSeed : ISeed
{
    public Task Seeding()
    {
        return Task.CompletedTask;
    }
}