using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.CompanyUserConsumer;

public class CompanyUserCreatedConsumer(
    ILogger<CompanyUserCreatedConsumer> logger,
    IMediator mediator)
    : BaseCreatedConsumer<CompanyUser, CompanyUserDto>(logger, mediator)
{
}