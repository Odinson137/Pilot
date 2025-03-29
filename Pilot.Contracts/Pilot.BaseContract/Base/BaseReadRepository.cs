using System.Data;
using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Services;
using Serialize.Linq.Serializers;

namespace Pilot.Contracts.Base;

public class BaseReadRepository<T>(DbContext context, IMapper mapper) : IBaseReadRepository<T>
    where T : BaseModel
{
    public DbSet<T> DbSet { get; } = context.Set<T>();

    public virtual async Task<T?> GetByIdAsync(int id, CancellationToken token = default)
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

    public virtual async Task<ICollection<TOut>> GetValuesAsync<TOut>(BaseFilter filter, CancellationToken token = default) where TOut : BaseDto
    {
        var query = DbSet
            .Skip(filter.Skip)
            .Take(filter.Take);

        query = query.OrderByDescending(c => c.Id);
        
        if (filter.Ids != null)
            query = query.Where(c => filter.Ids.Contains(c.Id));
        
        if (filter.WhereFilter != null)
            query = GetFiltersLambda(query, filter.WhereFilter.List);
        
        // if (filter.SelectQuery != null)
        //     query = GetSelectLambda(query, filter.SelectQuery);
        
        return await query
            .ProjectTo<TOut>(mapper.ConfigurationProvider)
            .ToListAsync(token);
    }

    public virtual async Task<string> GetQueryValuesAsync(BaseFilter filter, CancellationToken token = default)
    {
        var query = DbSet
            .Skip(filter.Skip)
            .Take(filter.Take);

        query = query.OrderByDescending(c => c.Id);
        
        if (filter.Ids != null)
            query = query.Where(c => filter.Ids.Contains(c.Id));
        
        if (filter.WhereFilter != null)
            query = GetFiltersLambda(query, filter.WhereFilter.List);
        
        // пока так
        var exp = GetSelectLambda(query, filter.SelectQuery!);

        var result = await query
            .Select(exp)
            .ToListAsync(token);
        return result.ToJson();
    }

    private static IQueryable<T> GetFiltersLambda(IQueryable<T> query, ICollection<(string, object, Type)> list)
    {
        foreach (var valueTuple in list)
            query = query.Where(GetFilterLambda(valueTuple));
        return query;
    }

    private static Expression<Func<T, bool>> GetFilterLambda((string, object, Type) filter)
    {
        var expNameParameter = Expression.Parameter(typeof(T), "e");
        var names = filter.Item1.Split('.');
        
        if (names.Length == 0)
            throw new NoNullAllowedException("Not correct property name");
        
        Expression expMember = expNameParameter;
        foreach (var name in names) expMember = Expression.Property(expMember, name);
        
        var value = Convert.ChangeType(filter.Item2, expMember.Type);
        var expValue = Expression.Constant(value);

        var eq = Expression.Equal(expMember!, expValue);

        var func = Expression.Lambda<Func<T, bool>>(eq, expNameParameter);
        return func;
    }
    
    private static Expression<Func<T, object>> GetSelectLambda(IQueryable<T> query, string queryString)
    {
        var serializer = new ExpressionSerializer(new JsonSerializer());
        var expression = serializer.DeserializeText(queryString) as Expression<Func<T, object>>;
        return expression!;
    }

}