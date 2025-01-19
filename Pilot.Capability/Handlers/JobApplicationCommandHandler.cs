using Pilot.Capability.Interface;
using Pilot.Capability.Models;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Handlers;

namespace Pilot.Capability.Handlers;

public class JobApplicationHandler : ModelQueryHandler<JobApplication, JobApplicationDto>
{
    public JobApplicationHandler(IJobApplication repository, ILogger<JobApplicationHandler> logger) : base(repository, logger)
    {
    }
}