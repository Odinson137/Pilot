using Pilot.Capability.Interface;
using Pilot.Capability.Models;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Handlers;

namespace Pilot.Capability.Handlers;

public class SkillQueryHandler : ModelQueryHandler<Skill, SkillDto>
{
    public SkillQueryHandler(ISkill repository, ILogger<SkillQueryHandler> logger) : base(repository, logger)
    {
    }
}