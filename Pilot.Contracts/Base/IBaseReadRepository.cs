using Microsoft.EntityFrameworkCore;

namespace Pilot.Contracts.Base;

public interface IBaseReadRepository<T> where T : BaseId
{
    public DbSet<T> DbSet { get; }
    public Task<T?> GetByIdAsync(int id, CancellationToken token = default);
    Task<T> GetRequiredByIdAsync(int id, CancellationToken token = default);
    public Task<TOut?> GetByIdAsync<TOut>(int id, CancellationToken token = default) where TOut : BaseId;
    public Task<ICollection<TOut>> GetAllValuesAsync<TOut>(int skip, int take, CancellationToken token = default);
    public Task<ICollection<T>> GetAllValuesAsync(int skip, int take, CancellationToken token = default);
}