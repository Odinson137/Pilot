using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Models;
using Pilot.SqrsControllerLibrary.Handlers;
using Pilot.Worker.Interface;

namespace Pilot.Worker.Handlers;

public class CompanyQueryHandler : ModelQueryHandler<Company, CompanyDto>
{
    public CompanyQueryHandler(ICompany repository, ILogger<CompanyQueryHandler> logger) : base(repository, logger)
    {
    }
}