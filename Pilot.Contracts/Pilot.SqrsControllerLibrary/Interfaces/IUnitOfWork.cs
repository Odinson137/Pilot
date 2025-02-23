namespace Pilot.SqrsControllerLibrary.Interfaces;

public interface IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken token = default);
}