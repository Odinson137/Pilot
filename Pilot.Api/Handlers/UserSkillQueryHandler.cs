using MediatR;
using Pilot.Api.Handlers.BaseHandlers;
using Pilot.Api.Queries;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data;
using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.Api.Handlers;

public class UserSkillQueryHandler(IModelService modelService) : ModelQueryHandler<UserSkillDto>(modelService), IRequestHandler<GetUserSkillsQuery, ICollection<UserSkillDto>>
{
    private readonly IModelService _modelService1 = modelService;

    public async Task<ICollection<UserSkillDto>> Handle(GetUserSkillsQuery request, CancellationToken cancellationToken)
    {
        var userDto = await _modelService1.GetValuesAsync<UserSkillDto>($"{Urls.UserSkills}/{request.UserId}", null, cancellationToken);
        return userDto;
    }
}