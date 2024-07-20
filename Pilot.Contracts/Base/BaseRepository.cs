using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Exception.ProjectExceptions;

namespace Pilot.Contracts.Base;

public abstract class BaseRepository<T>(DbContext context, IMapper mapper) : BaseReadRepository<T>(context, mapper), IBaseRepository<T>
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
        await _context.SaveChangesAsync(token);
    }

    public void DeleteAsync(T value)
    {
        _context.Remove(value);
    }

    public DbContext GetContext => _context;
}