namespace Pilot.Contracts.Base;

public interface IBaseReadRepository<T>
{
    public Task<T?> GetByIdAsync(int id, CancellationToken token = default);
    public Task<TOut?> GetByIdAsync<TOut>(int id, CancellationToken token = default) where TOut : BaseId;
    public Task<ICollection<TOut>> GetAllValuesAsync<TOut>(int skip, int take, CancellationToken token = default);
    public Task<ICollection<T>> GetAllValuesAsync(int skip, int take, CancellationToken token = default);
}