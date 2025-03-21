﻿using AutoMapper;
using Pilot.Capability.Consumers.Base;
using Pilot.Capability.Interface;
using Pilot.Capability.Models;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;

namespace Pilot.Capability.Consumers.SkillConsumer;

public class SkillUpdatedConsumer(
    ILogger<SkillUpdatedConsumer> logger,
    ISkill repository,
    IMessageService message,
    IValidatorService validate,
    IMapper mapper)
    : BaseUpdateConsumer<Skill, SkillDto>(logger, repository, message, validate, mapper)
{
}