using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Pilot.Contracts.Base;

public class BaseReadRepository<T>(DbContext context, IMapper mapper) : IBaseReadRepository<T>
    where T : BaseId
{
    protected readonly DbSet<T> DbSet = context.Set<T>();
    
    public async Task<T?> GetByIdAsync(int id, CancellationToken token = default)
    {
        return await DbSet.FirstOrDefaultAsync(c => c.Id == id, token);
    }
    
    public async Task<TOut?> GetByIdAsync<TOut>(int id, CancellationToken token = default) where TOut : BaseId
    {
        return await DbSet.ProjectTo<TOut>(mapper.ConfigurationProvider).FirstOrDefaultAsync(c => c.Id == id, token);
    }

    public async Task<ICollection<T>> GetAllValuesAsync(int skip, int take, CancellationToken token = default)
    {
        return await GetAllValuesAsync<T>(skip, take, token);
    }
    
    public async Task<ICollection<TOut>> GetAllValuesAsync<TOut>(int skip, int take, CancellationToken token = default)
    {
        return await DbSet.Skip(skip).Take(take).OrderByDescending(c => c.Id).ProjectTo<TOut>(mapper.ConfigurationProvider).ToListAsync(token);
    }
}