using Microsoft.EntityFrameworkCore;

namespace Pilot.Contracts.Base;

public interface IBaseReadRepository<T> where T : BaseModel
{
    public DbSet<T> DbSet { get; }
    
    public Task<T?> GetByIdAsync(int id, CancellationToken token = default);
    
    Task<T> GetRequiredByIdAsync(int id, CancellationToken token = default);

    public Task<TOut> GetRequiredByIdAsync<TOut>(int id, CancellationToken token = default) where TOut : BaseDto;
    
    public Task<TOut?> GetByIdAsync<TOut>(int id, CancellationToken token = default) where TOut : BaseDto;
    
    public Task<ICollection<TOut>> GetValuesAsync<TOut>(BaseFilter? filter, CancellationToken token = default) where TOut : BaseDto;
}