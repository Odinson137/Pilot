using Pilot.Contracts.DTO.ModelDto;
using Pilot.Receiver.Interface;
using Pilot.Receiver.Models;
using Pilot.SqrsControllerLibrary.Handlers;

namespace Pilot.Receiver.Handlers;

public class CompanyUserHandler : ModelQueryHandler<CompanyUser, CompanyUserDto>
{
    public CompanyUserHandler(ICompanyUser repository, ILogger<ModelQueryHandler<CompanyUser, CompanyUserDto>> logger) : base(repository, logger)
    {
    }
}