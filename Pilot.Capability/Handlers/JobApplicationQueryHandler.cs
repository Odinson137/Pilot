using Pilot.Capability.Interface;
using Pilot.Capability.Models;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Handlers;

namespace Pilot.Capability.Handlers;

public class JobApplicationQueryHandler : ModelQueryHandler<JobApplication, JobApplicationDto>
{
    public JobApplicationQueryHandler(IJobApplication repository, ILogger<JobApplicationQueryHandler> logger) : base(repository, logger)
    {
    }
}