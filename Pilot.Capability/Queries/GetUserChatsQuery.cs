using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Queries;

namespace Pilot.Capability.Queries;

public record GetUserSkillQuery(int UserId)
    : BaseQuery, IRequest<ICollection<UserSkillDto>>;

