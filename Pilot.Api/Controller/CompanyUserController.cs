using MediatR;
using Pilot.Api.Data.ControllerSettings;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Models;

namespace Pilot.Api.Controller;

public class CompanyUserController(IMediator mediator) : PilotController<CompanyUser, CompanyUserDto>(mediator);
