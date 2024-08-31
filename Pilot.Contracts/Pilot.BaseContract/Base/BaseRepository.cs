using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Pilot.Contracts.Base;

public abstract class BaseRepository<T>(DbContext context, IMapper mapper)
    : BaseReadRepository<T>(context, mapper), IBaseRepository<T>
    where T : BaseModel
{
    private readonly DbContext _context = context;

    public async Task<T> AddValueToContextAsync(T value, CancellationToken token = default)
    {
        await DbSet.AddAsync(value, token);
        return value;
    }

    public async Task SaveAsync(CancellationToken token = default)
    {
        await GetContext.SaveChangesAsync(token);
    }

    public async Task<int> FastDeleteAsync(int modelId, CancellationToken token = default)
    {
        return await DbSet.Where(c => c.Id == modelId).ExecuteDeleteAsync(token);
    }

    public void DeleteAsync(T value)
    {
        GetContext.Remove(value);
    }

    public DbContext GetContext { get; } = context;
}