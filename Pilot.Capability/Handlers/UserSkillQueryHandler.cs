using MediatR;
using Pilot.Capability.Interface;
using Pilot.Capability.Models;
using Pilot.Capability.Queries;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Services.LogService;
using Pilot.SqrsControllerLibrary.Handlers;

namespace Pilot.Capability.Handlers;

public class UserSkillQueryHandler(IUserSkill repository, ILogger<UserSkillQueryHandler> logger)
    : ModelQueryHandler<UserSkill, UserSkillDto>(repository, logger), IRequestHandler<GetUserSkillQuery, ICollection<UserSkillDto>>
{
    public async Task<ICollection<UserSkillDto>> Handle(GetUserSkillQuery request, CancellationToken cancellationToken)
    {
        Logger.LogClassInfo("Get user skills");
        var result = await repository.GetUserSkillsAsync(request.UserId, cancellationToken);
        return result;
    }
}