using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Pilot.Capability.Data;
using Pilot.Capability.Interface;
using Pilot.Capability.Models;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.Capability.Repository;

public class CompanyPostRepository(DataContext context, IMapper mapper) : BaseRepository<CompanyPost>(context, mapper), ICompanyPost
{
    public async Task<ICollection<CompanyPostDto>> GetOpenPostsAsync(BaseFilter filter, CancellationToken token)
    {
        var query = DbSet
            .Where(c => c.Post.Id == filter.Ids!.First())
            .Skip(filter.Skip)
            .Take(filter.Take)
            .OrderByDescending(c => c.Id)
            .ProjectTo<CompanyPostDto>(mapper.ConfigurationProvider);

        if (filter.Ids != null)
        {
            query = (IOrderedQueryable<CompanyPostDto>)query.Where(c => filter.Ids.Contains(c.Id));
        }
    
        return await query.ToListAsync(token);
    }
}