using Pilot.Contracts.DTO.ModelDto;
using Pilot.Receiver.Interface;
using Pilot.Receiver.Models;
using Pilot.SqrsControllerLibrary.Handlers;

namespace Pilot.Receiver.Handlers;

public class CompanyRoleHandler : ModelQueryHandler<CompanyRole, CompanyRoleDto>
{
    public CompanyRoleHandler(ICompanyRole repository, ILogger<CompanyRoleHandler> logger) : base(repository, logger)
    {
    }
}