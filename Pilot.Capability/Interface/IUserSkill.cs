using Pilot.Capability.Models;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.Capability.Interface;

public interface IUserSkill : IBaseRepository<UserSkill>
{
    Task<ICollection<UserSkillDto>> GetUserSkillsAsync(int userId, CancellationToken token);
}