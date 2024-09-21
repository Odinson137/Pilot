using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Handlers;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Handlers;

public class CompanyRoleHandler : ModelQueryHandler<CompanyRole, CompanyRoleDto>
{
    public CompanyRoleHandler(ICompanyRole repository, ILogger<CompanyRoleHandler> logger) : base(repository, logger)
    {
    }
}