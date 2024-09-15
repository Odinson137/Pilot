using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Receiver.Interface;
using Pilot.Receiver.Models;
using Pilot.SqrsControllerLibrary.Handlers;

namespace Pilot.Receiver.Handlers;

public class CompanyHandler : ModelQueryHandler<Company, CompanyDto>
{
    public CompanyHandler(ICompany repository, ILogger<CompanyHandler> logger) : base(repository, logger)
    {
    }
}