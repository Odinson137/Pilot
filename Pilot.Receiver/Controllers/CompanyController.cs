using Pilot.Contracts.DTO.ModelDto;
using Pilot.Receiver.Interface;
using Pilot.Receiver.Models;

namespace Pilot.Receiver.Controllers;

public class CompanyController(ICompany repository, ILogger<CompanyController> logger)
    : BaseReadOnlyController<Company, CompanyDto>(repository, logger);