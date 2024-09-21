using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Worker.Models;
using Pilot.SqrsControllerLibrary.Handlers;
using Pilot.Worker.Interface;

namespace Pilot.Worker.Handlers;

public class CompanyHandler : ModelQueryHandler<Models.Company, CompanyDto>
{
    public CompanyHandler(ICompany repository, ILogger<CompanyHandler> logger) : base(repository, logger)
    {
    }
}