using MediatR;
using Pilot.Contracts.DTO;

namespace Pilot.Api.Commands;

public record UserRegistrationCommand(RegistrationUserDto UserDto) : IRequest;

public record UserAuthorizationCommand(AuthorizationUserDto UserDto) : IRequest<AuthUserDto>;

