using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Controller;

namespace Pilot.Api.Controller;

public class CompanyUserController(IMediator mediator) : PilotController<CompanyUserDto>(mediator);