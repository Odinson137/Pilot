using MediatR;
using Pilot.Api.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Models;

namespace Pilot.Api.Controller;

public class CompanyController(IMediator mediator) : GatewayController<Company, CompanyDto>(mediator);
