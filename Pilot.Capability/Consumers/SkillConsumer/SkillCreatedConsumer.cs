using AutoMapper;
using Pilot.Capability.Consumers.Base;
using Pilot.Capability.Interface;
using Pilot.Capability.Models;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;

namespace Pilot.Capability.Consumers.SkillConsumer;

public class SkillConsumer(
    ILogger<SkillConsumer> logger,
    ISkill repository,
    IMessageService messageService,
    IValidatorService validate,
    IMapper mapper)
    : BaseCreatedConsumer<Skill, SkillDto>(logger, repository, messageService, validate, mapper)
{
}