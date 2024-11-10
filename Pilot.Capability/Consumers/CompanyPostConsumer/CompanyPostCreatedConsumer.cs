using AutoMapper;
using Pilot.Capability.Consumers.Base;
using Pilot.Capability.Interface;
using Pilot.Capability.Models;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;

namespace Pilot.Capability.Consumers.CompanyPostConsumer;

public class CompanyPostConsumer(
    ILogger<CompanyPostConsumer> logger,
    ICompanyPost repository,
    IMessageService messageService,
    IValidatorService validate,
    IMapper mapper)
    : BaseCreatedConsumer<CompanyPost, CompanyPostDto>(logger, repository, messageService, validate, mapper)
{
}