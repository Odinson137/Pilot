using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Models;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Controllers;

public class CompanyController(ICompany repository, ILogger<CompanyController> logger) : BaseSelectController<Company, CompanyDto>(repository, logger);