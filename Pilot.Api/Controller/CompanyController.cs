﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pilot.Api.Data.ControllerSettings;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Models;

namespace Pilot.Api.Controller;

[Route("api/[controller]")]
[ApiController]
public class CompanyController : PilotController<Company, CompanyDto>
{
    private readonly IMediator _mediator;

    public CompanyController(IMediator mediator) : base(mediator)
    {
        _mediator = mediator;
    }
}