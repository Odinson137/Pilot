using MediatR;
using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.Api.Queries;

public record GetUserSkillsQuery(int UserId) : IRequest<ICollection<UserSkillDto>>;
