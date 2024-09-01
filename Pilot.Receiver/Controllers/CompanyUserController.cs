using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Controller;

namespace Pilot.Receiver.Controllers;

public class CompanyUserController(Mediator mediator)
    : PilotReadOnlyController<CompanyUserDto>(mediator);