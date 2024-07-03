using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Exception.ProjectExceptions;

namespace Pilot.Contracts.Base;

public abstract class BaseRepository<T>(DbContext context, IMapper mapper) : BaseReadRepository<T>(context, mapper), IBaseRepository<T>
    where T : BaseId
{
    private readonly IMapper _mapper = mapper;
    private readonly DbContext _context = context;

    public async Task<T> AddNewValueAsync(T value, CancellationToken token = default)
    {
        await DbSet.AddAsync(value, token);
        return value;
    }
    
    public async Task<T> AddNewValueAsync<TIn>(TIn value, CancellationToken token = default)
    {
        var outValue = _mapper.Map<T>(value);
        await DbSet.AddAsync(outValue, token);
        return outValue;
    }

    public async Task SaveAsync(CancellationToken token = default)
    {
        await _context.SaveChangesAsync(token);
    }

    public void DeleteAsync(T value)
    {
        _context.Remove(value);
    }

    public async Task DeleteAsync(int valueId)
    {
        var value = await DbSet.FirstOrDefaultAsync(c => c.Id == valueId);
        if (value == null)
        {
            throw new NotFoundException($"The value by id {valueId} of {nameof(T)} does not found");
        }
        _context.Remove(value);
    }
    
    public DbContext GetContext()
    {
        return _context;
    }
}