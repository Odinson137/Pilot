using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Exception.ProjectExceptions;

namespace Pilot.Contracts.Base;

public abstract class BaseRepository<T>(DbContext context, MapperConfiguration configuration) : IBaseRepository<T>
    where T : BaseId
{
    protected readonly DbSet<T> DbSet = context.Set<T>();

    
    public async Task<T?> GetByIdAsync(int id, CancellationToken token = default)
    {
        return await GetByIdAsync<T>(id, token);
    }
    
    public async Task<TOut?> GetByIdAsync<TOut>(int id, CancellationToken token = default) where TOut : BaseId
    {
        return await DbSet.ProjectTo<TOut>(configuration).FirstOrDefaultAsync(c => c.Id == id, token);
    }

    public async Task<ICollection<T>> GetAllValuesAsync(int skip, int take , CancellationToken token = default)
    {
        return await GetAllValuesAsync<T>(skip, take, token);
    }
    
    public async Task<ICollection<TOut>> GetAllValuesAsync<TOut>(int skip, int take , CancellationToken token = default)
    {
        return await DbSet.Skip(skip).Take(take).ProjectTo<TOut>(configuration).ToListAsync(token);
    }

    public async Task<T> AddNewValueAsync(T value, CancellationToken token = default)
    {
        await DbSet.AddAsync(value, token);
        return value;
    }
    
    public async Task<T> AddNewValueAsync<TIn>(TIn value, CancellationToken token = default)
    {
        var vas = configuration.CreateMapper();
        var outValue = vas.Map<T>(value);
        await DbSet.AddAsync(outValue, token);
        return outValue;
    }

    public async Task SaveAsync(CancellationToken token = default)
    {
        await context.SaveChangesAsync(token);
    }

    public void DeleteAsync(T value)
    {
        context.Remove(value);
    }

    public async Task DeleteAsync(int valueId)
    {
        var value = await DbSet.FirstOrDefaultAsync(c => c.Id == valueId);
        if (value == null)
        {
            throw new NotFoundException($"The value by id {valueId} of {nameof(T)} does not found");
        }
        context.Remove(value);
    }
    
    public DbContext GetContext()
    {
        return context;
    }
}