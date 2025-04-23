using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pilot.Capability.Data;
using Pilot.Capability.Interface;
using Pilot.Capability.Models;
using Pilot.Contracts.Base;

namespace Pilot.Capability.Repository;

public class SkillRepository(DataContext context, IMapper mapper) : BaseRepository<Skill>(context, mapper), ISkill
{
    public async Task<ICollection<Skill>> GetSkillsAsync(params int[] ids)
    {
        return await context.Skills.Where(c => ids.Contains(c.Id)).ToListAsync();
    }
}