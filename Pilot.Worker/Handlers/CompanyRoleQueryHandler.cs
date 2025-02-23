using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Handlers;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Handlers;

public class CompanyRoleQueryHandler : ModelQueryHandler<CompanyRole, CompanyRoleDto>
{
    public CompanyRoleQueryHandler(ICompanyRole repository, ILogger<CompanyRoleQueryHandler> logger) : base(repository, logger)
    {
    }
}