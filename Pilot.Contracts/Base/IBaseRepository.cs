using Microsoft.EntityFrameworkCore;

namespace Pilot.Contracts.Base;

public interface IBaseRepository<T> : IBaseReadRepository<T> where T : BaseId
{
    public Task<T> AddNewValueAsync(T value, CancellationToken token = default);
    
    public Task SaveAsync(CancellationToken token = default);
    
    public void DeleteAsync(T value);
    
    public DbContext GetContext { get; }
}