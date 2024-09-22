using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Models;
using Pilot.SqrsControllerLibrary.Handlers;
using Pilot.Worker.Interface;

namespace Pilot.Worker.Handlers;

public class CompanyHandler : ModelQueryHandler<Company, CompanyDto>
{
    public CompanyHandler(ICompany repository, ILogger<CompanyHandler> logger) : base(repository, logger)
    {
    }
}