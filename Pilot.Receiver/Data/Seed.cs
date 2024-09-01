using Pilot.Contracts.Data;

namespace Pilot.Receiver.Data;

public class Seed : ISeed
{
    public Task Seeding()
    {
        return Task.CompletedTask;
    }
}