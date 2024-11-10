using AutoMapper;
using Pilot.Capability.Consumers.Base;
using Pilot.Capability.Interface;
using Pilot.Capability.Models;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;

namespace Pilot.Capability.Consumers.CompanyPostConsumer;

public class CompanyPostUpdatedConsumer(
    ILogger<CompanyPostUpdatedConsumer> logger,
    ICompanyPost repository,
    IMessageService message,
    IValidatorService validate,
    IMapper mapper)
    : BaseUpdateConsumer<CompanyPost, CompanyPostDto>(logger, repository, message, validate, mapper)
{
}