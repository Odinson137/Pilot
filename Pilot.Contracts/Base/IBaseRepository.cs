using Microsoft.EntityFrameworkCore;

namespace Pilot.Contracts.Base;

public interface IBaseRepository<T> : IBaseSelectRepository<T>
{
    public Task<T> AddNewValueAsync<TIn>(TIn value, CancellationToken token = default);
    public Task<T> AddNewValueAsync(T value, CancellationToken token = default);
    public Task SaveAsync(CancellationToken token = default);
    public Task DeleteAsync(int valueId);
    public void DeleteAsync(T value);
    public DbContext GetContext();
}