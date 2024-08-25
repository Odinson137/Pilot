using Pilot.Contracts.DTO.ModelDto;
using Pilot.Receiver.Interface;
using Pilot.Receiver.Models;

namespace Pilot.Receiver.Controllers;

public class CompanyUserController(ICompanyUser repository, ILogger<CompanyUserController> logger)
    : BaseReadOnlyController<CompanyUser, CompanyUserDto>(repository, logger);