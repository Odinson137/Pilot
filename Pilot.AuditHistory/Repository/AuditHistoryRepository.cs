using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Pilot.AuditHistory.Data;
using Pilot.AuditHistory.Interface;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;

namespace Pilot.AuditHistory.Repository;

public class AuditHistoryRepository(ClickHouseContext context, IMapper mapper) : BaseRepository<Models.AuditHistory>(context, mapper), IAuditHistory
{
    public async Task<Models.AuditHistory?> Get(int entityId, ModelType entityType, CancellationToken token = default)
    {
        return await DbSet.FirstOrDefaultAsync(c => c.EntityId == entityId && c.EntityType == entityType, token);
    }

    public async Task<Models.AuditHistory?> GetValueByKey(int entityId, ModelType entityType, CancellationToken token = default)
    {
        return await DbSet.FirstOrDefaultAsync(c => c.EntityId == entityId && c.EntityType == entityType, token);
    }

    public override async Task<ICollection<TOut>> GetValuesAsync<TOut>(BaseFilter filter, CancellationToken token = default)
    {
        var query = DbSet
            .Skip(filter.Skip)
            .Take(filter.Take);
        
        return await query
            .ProjectTo<TOut>(mapper.ConfigurationProvider)
            .OrderByDescending(c => c.CreateAt)
            .ToListAsync(token);
    }
}