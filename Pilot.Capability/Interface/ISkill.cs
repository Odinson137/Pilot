using Pilot.Capability.Models;
using Pilot.Contracts.Base;

namespace Pilot.Capability.Interface;

public interface ISkill : IBaseRepository<Skill>
{
    public Task<ICollection<Skill>> GetSkillsAsync(params int[] ids);
}