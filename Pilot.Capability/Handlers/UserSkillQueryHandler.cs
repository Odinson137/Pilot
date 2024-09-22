using Pilot.Capability.Interface;
using Pilot.Capability.Models;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Handlers;

namespace Pilot.Capability.Handlers;

public class UserSkillQueryHandler : ModelQueryHandler<UserSkill, UserSkillDto>
{
    public UserSkillQueryHandler(IUserSkill repository, ILogger<UserSkillQueryHandler> logger) : base(repository, logger)
    {
    }
}