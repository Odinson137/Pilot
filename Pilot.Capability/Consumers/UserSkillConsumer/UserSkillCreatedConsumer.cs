using AutoMapper;
using Pilot.Capability.Consumers.Base;
using Pilot.Capability.Interface;
using Pilot.Capability.Models;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;

namespace Pilot.Capability.Consumers.UserSkillConsumer;

public class UserSkillCreatedConsumer(
    ILogger<UserSkillCreatedConsumer> logger,
    IUserSkill repository,
    IMessageService messageService,
    IValidatorService validate,
    IMapper mapper)
    : BaseCreatedConsumer<UserSkill, UserSkillDto>(logger, repository, messageService, validate, mapper)
{
}