using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Identity.Models;
using Pilot.SqrsControllerLibrary.Consumers.Base;

namespace Pilot.Identity.Consumers.UserConsumer;

public class UserUpdatedConsumer(
    ILogger<UserUpdatedConsumer> logger,
    IMediator mediator)
    : BaseUpdateConsumer<User, UserDto>(logger, mediator)
{
}