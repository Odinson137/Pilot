using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Handlers;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Handlers;

public class CompanyUserQueryHandler : ModelQueryHandler<CompanyUser, CompanyUserDto>
{
    public CompanyUserQueryHandler(ICompanyUser repository, ILogger<ModelQueryHandler<CompanyUser, CompanyUserDto>> logger) : base(repository, logger)
    {
    }
}