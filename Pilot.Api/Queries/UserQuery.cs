using MediatR;
using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.Api.Queries;

public record GetUserQuery(int UserId) : IRequest<UserDto>;

