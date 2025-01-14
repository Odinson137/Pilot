using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Exception.ApiExceptions;
using Pilot.Storage.Data;
using Pilot.Storage.Interface;
using File = Pilot.Storage.Models.File;

namespace Pilot.Storage.Repository;

public class FileRepository(DataContext context, IMapper mapper)
    : BaseRepository<File>(context, mapper), IFileRepository
{
    public async Task<ICollection<FileDto>> GetValuesUrlAsync(ICollection<string> names,
        CancellationToken token = default)
    {
        var query = DbSet
            .Where(c => names.Contains(c.Name))
            .ProjectTo<FileDto>(mapper.ConfigurationProvider);

        return await query.ToListAsync(token);
    }

    public async Task<FileDto> GetValueUrlAsync(string name, CancellationToken token = default)
    {
        var query = DbSet
            .Where(c => c.Name == name)
            .ProjectTo<FileDto>(mapper.ConfigurationProvider);

        var result = await query.FirstOrDefaultAsync(token);
        if (result == null) throw new NotFoundException("File not found");
        return result;
    }
}