using MediatR;
using Pilot.Contracts.DTO;

namespace Pilot.Api.Commands;

public record UserRegistrationCommand(string UserName, string Name, string LastName, string Password) : IRequest;

public record UserAuthorizationCommand(string UserName, string Password) : IRequest<AuthUserDto>;

