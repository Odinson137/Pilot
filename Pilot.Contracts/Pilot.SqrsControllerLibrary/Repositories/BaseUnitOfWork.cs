using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pilot.SqrsControllerLibrary.Interfaces;

namespace Pilot.SqrsControllerLibrary.Repositories;

public abstract class BaseUnitOfWork(ILogger<BaseUnitOfWork> logger, DbContext context) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken token = default)
    {
        var entries = context.ChangeTracker.Entries().Select(e => $"{e.Entity} - State: {e.State}");
        logger.LogInformation("Entities in context: " + string.Join(", ", entries));
        return context.SaveChangesAsync(token);
    }
}