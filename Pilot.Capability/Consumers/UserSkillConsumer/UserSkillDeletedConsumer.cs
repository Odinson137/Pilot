﻿using Pilot.Capability.Consumers.Base;
using Pilot.Capability.Interface;
using Pilot.Capability.Models;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;

namespace Pilot.Capability.Consumers.UserSkillConsumer;

public class UserSkillDeletedConsumer(
    ILogger<UserSkillDeletedConsumer> logger,
    IUserSkill repository,
    IMessageService message,
    IValidatorService validate)
    : BaseDeleteConsumer<UserSkill, UserSkillDto>(logger, repository, message, validate)
{
}