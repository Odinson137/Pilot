using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Pilot.Capability.Data;
using Pilot.Capability.Interface;
using Pilot.Capability.Models;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.Capability.Repository;

public class UserSkillRepository(DataContext context, IMapper mapper) : BaseRepository<UserSkill>(context, mapper), IUserSkill
{
    private readonly IMapper _mapper = mapper;

    public async Task<ICollection<UserSkillDto>> GetUserSkillsAsync(int userId, CancellationToken token)
    {
        var query = DbSet
            .Where(c => c.UserId == userId)
            .ProjectTo<UserSkillDto>(_mapper.ConfigurationProvider);

        return await query.ToListAsync(token);
    }
}