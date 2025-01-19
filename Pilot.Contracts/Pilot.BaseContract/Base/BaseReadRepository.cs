using System.Data;
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
            query = query.Where(c => filter.Ids.Contains(c.Id));
        
        if (filter.WhereFilter != null)
            query = GetFiltersLambda(query, filter.WhereFilter.List);
        
        return await query.ToListAsync(token);
    }

    private static IQueryable<TOut> GetFiltersLambda<TOut>(IQueryable<TOut> query, ICollection<(string, object, Type)> list)
    {
        foreach (var valueTuple in list)
            query = query.Where(GetFilterLambda<TOut>(valueTuple));
        return query;
    }

    private static Expression<Func<TOut, bool>> GetFilterLambda<TOut>((string, object, Type) filter)
    {
        var expNameParameter = Expression.Parameter(typeof(TOut), "e");
        var names = filter.Item1.Split('.');
        
        if (names.Length == 0)
            throw new NoNullAllowedException("Not correct property name");
        
        Expression expMember = expNameParameter;
        foreach (var name in names) expMember = Expression.Property(expMember, name);
        
        var value = Convert.ChangeType(filter.Item2, expMember.Type);
        var expValue = Expression.Constant(value);

        var eq = Expression.Equal(expMember!, expValue);

        var func = Expression.Lambda<Func<TOut, bool>>(eq, expNameParameter);
        return func;
    }
}