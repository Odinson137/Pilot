using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Services;
using Pilot.SqrsControllerLibrary.Controller;
using Pilot.SqrsControllerLibrary.Queries;

namespace Pilot.Api.Controller;

public class CompanyRoleController(IMediator mediator) : PilotReadOnlyController<CompanyRoleDto>(mediator)
{
}