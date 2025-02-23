using Pilot.SqrsControllerLibrary.Interfaces;
using Pilot.Worker.Data;

namespace Pilot.Worker.Repository;

public class UnitOfWork(ILogger<UnitOfWork> logger, DataContext context) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken token = default)
    {
        var entries = context.ChangeTracker.Entries().Select(e => $"{e.Entity} - State: {e.State}");
        logger.LogInformation("Entities in context: " + string.Join(", ", entries));
        return context.SaveChangesAsync(token);
    }
}