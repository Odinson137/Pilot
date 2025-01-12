using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Base;
using Pilot.Storage.Data;
using Pilot.Storage.Interface;
using File = Pilot.Storage.Models.File;

namespace Pilot.Storage.Repository;

public class FileRepository(DataContext context, IMapper mapper)
    : BaseRepository<File>(context, mapper), IFileRepository
{
    public async Task<ICollection<TOut>> GetValuesUrlAsync<TOut>(ICollection<string> names,
        CancellationToken token = default) where TOut : BaseDto
    {
        var query = DbSet
            .Where(c => names.Contains(c.Name))
            .ProjectTo<TOut>(mapper.ConfigurationProvider);

        return await query.ToListAsync(token);
    }
}