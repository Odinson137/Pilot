using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Identity.Models;
using Pilot.SqrsControllerLibrary.Consumers.Base;

namespace Pilot.Identity.Consumers.UserConsumer;

public class UserDeletedConsumer(
    ILogger<UserDeletedConsumer> logger,
    IMediator mediator)
    : BaseDeleteConsumer<User, UserDto>(logger, mediator)
{
}