using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.CompanyUserConsumer;

public class CompanyUserDeletedConsumer(
    ILogger<CompanyUserDeletedConsumer> logger,
    IMediator mediator)
    : BaseDeleteConsumer<CompanyUser, CompanyUserDto>(logger, mediator);