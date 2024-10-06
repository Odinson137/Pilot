using MediatR;
using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.Api.Commands;

public record GetUserQuery(int UserId) : IRequest<UserDto>;

