using Microsoft.EntityFrameworkCore.Storage;

namespace Pilot.SqrsControllerLibrary.Interfaces;

public interface IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken token = default);
    public Task<IDbContextTransaction> StartTransactionAsync(CancellationToken token = default);
    public Task EndTransactionAsync(CancellationToken token = default);
    public bool HasActiveTransaction { get; }
}