using Pilot.Capability.Interface;
using Pilot.Capability.Models;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Handlers;

namespace Pilot.Capability.Handlers;

public class UserSkillQueryHandler : ModelQueryHandler<Skill, SkillDto>
{
    public UserSkillQueryHandler(ISkill repository, ILogger<UserSkillQueryHandler> logger) : base(repository, logger)
    {
    }
}