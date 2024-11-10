using AutoMapper;
using Pilot.Capability.Consumers.Base;
using Pilot.Capability.Interface;
using Pilot.Capability.Models;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;

namespace Pilot.Capability.Consumers.UserSkillConsumer;

public class UserSkillUpdatedConsumer(
    ILogger<UserSkillUpdatedConsumer> logger,
    IUserSkill repository,
    IMessageService message,
    IValidatorService validate,
    IMapper mapper)
    : BaseUpdateConsumer<UserSkill, UserSkillDto>(logger, repository, message, validate, mapper)
{
}