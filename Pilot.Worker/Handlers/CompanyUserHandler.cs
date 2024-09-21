using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Handlers;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Handlers;

public class CompanyUserHandler : ModelQueryHandler<CompanyUser, CompanyUserDto>
{
    public CompanyUserHandler(ICompanyUser repository, ILogger<ModelQueryHandler<CompanyUser, CompanyUserDto>> logger) : base(repository, logger)
    {
    }
}