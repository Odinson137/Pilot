using AutoMapper;
using Pilot.Capability.Consumers.Base;
using Pilot.Capability.Interface;
using Pilot.Capability.Models;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;

namespace Pilot.Capability.Consumers.JobApplicationConsumer;

public class JobApplicationCreatedConsumer(
    ILogger<JobApplicationCreatedConsumer> logger,
    IJobApplication repository,
    IMessageService messageService,
    IValidatorService validate,
    IMapper mapper)
    : BaseCreatedConsumer<JobApplication, JobApplicationDto>(logger, repository, messageService, validate, mapper)
{
}