using Microsoft.EntityFrameworkCore;

namespace Pilot.Contracts.Base;

public interface IBaseRepository<T> : IBaseReadRepository<T> where T : BaseModel
{
    public DbContext GetContext { get; }
    public Task<T> AddValueToContextAsync(T value, CancellationToken token = default);

    public Task SaveAsync(CancellationToken token = default);

    public void LazyLoading(bool isActive = false);

    public Task<int> FastDeleteAsync(int modelId, CancellationToken token = default);
    
    public void DeleteAsync(T value);
}