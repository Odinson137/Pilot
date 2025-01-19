using AutoMapper;
using Pilot.Capability.Consumers.Base;
using Pilot.Capability.Interface;
using Pilot.Capability.Models;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;

namespace Pilot.Capability.Consumers.JobApplicationConsumer;

public class JobApplicationUpdatedConsumer(
    ILogger<JobApplicationUpdatedConsumer> logger,
    IJobApplication repository,
    IMessageService message,
    IValidatorService validate,
    IMapper mapper)
    : BaseUpdateConsumer<JobApplication, JobApplicationDto>(logger, repository, message, validate, mapper)
{
}