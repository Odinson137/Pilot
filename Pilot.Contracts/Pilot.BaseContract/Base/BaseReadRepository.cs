using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Pilot.Contracts.Base;

public class BaseReadRepository<T>(DbContext context, IMapper mapper) : IBaseReadRepository<T>
    where T : BaseModel
{
    public DbSet<T> DbSet { get; } = context.Set<T>();

    public async Task<T?> GetByIdAsync(int id, CancellationToken token = default)
    {
        return await DbSet.FirstOrDefaultAsync(c => c.Id == id, token);
    }

    public async Task<T> GetRequiredByIdAsync(int id, CancellationToken token = default)
    {
        var value = await GetByIdAsync(id, token);
        return value ?? throw new NullReferenceException("Сущность по id не найдена");
    }
    
    public async Task<TOut> GetRequiredByIdAsync<TOut>(int id, CancellationToken token = default) where TOut : BaseDto
    {
        var value = await GetByIdAsync<TOut>(id, token);
        return value ?? throw new NullReferenceException("Сущность по id не найдена");
    }

    public async Task<TOut?> GetByIdAsync<TOut>(int id, CancellationToken token = default) where TOut : BaseDto
    {
        return await DbSet.ProjectTo<TOut>(mapper.ConfigurationProvider).FirstOrDefaultAsync(c => c.Id == id, token);
    }

    public async Task<ICollection<TOut>> GetValuesAsync<TOut>(BaseFilter? filter, CancellationToken token = default) where TOut : BaseDto
    {
        filter ??= new BaseFilter(0, int.MaxValue);
        var query = DbSet
            .Skip(filter.Skip)
            .Take(filter.Take)
            .OrderByDescending(c => c.Id) // TODO потом сделать динамическую фильтрацию
            .ProjectTo<TOut>(mapper.ConfigurationProvider);

        if (filter.Ids != null)
        {
            query = query.Where(c => filter.Ids.Contains(c.Id));
        }
        
        if (filter.WhereFilter.HasValue)
        {
            query = query.Where(GetFilterLambda<TOut>(filter.WhereFilter.Value));
        }
        
        return await query.ToListAsync(token);
    }

    private static Expression<Func<TOut, bool>> GetFilterLambda<TOut>((string, int) filter)
    {
        var expNameParameter = Expression.Parameter(typeof(TOut), "e");
        var expMember = Expression.Property(expNameParameter, filter.Item1);
        var expValue = Expression.Constant(filter.Item2);

        var eq = Expression.Equal(expMember, expValue);

        var func = Expression.Lambda<Func<TOut, bool>>(eq, expNameParameter);
        return func;
    }
}