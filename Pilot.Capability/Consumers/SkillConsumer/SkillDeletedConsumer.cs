using Pilot.Capability.Consumers.Base;
using Pilot.Capability.Interface;
using Pilot.Capability.Models;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;

namespace Pilot.Capability.Consumers.SkillConsumer;

public class SkillDeletedConsumer(
    ILogger<SkillDeletedConsumer> logger,
    ISkill repository,
    IMessageService message,
    IValidatorService validate)
    : BaseDeleteConsumer<Skill, SkillDto>(logger, repository, message, validate)
{
}