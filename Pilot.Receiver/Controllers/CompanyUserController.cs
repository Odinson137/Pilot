using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Models;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Controllers;

public class CompanyUserController(ICompanyUser repository, ILogger<CompanyUserController> logger) : BaseSelectController<CompanyUser, CompanyUserDto>(repository, logger);