using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pilot.Api.Queries;
using Pilot.Contracts.Data;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Controller;

namespace Pilot.Api.Controller;

public class UserSkillController(IMediator mediator) : PilotController<UserSkillDto>(mediator)
{
    [HttpGet(Urls.UserSkills + "/{userId:int}")]
    [ProducesResponseType(200)]
    public async Task<ICollection<UserSkillDto>> GetUserSkills(int userId, CancellationToken token)
    {
        var result = await Mediator.Send(new GetUserSkillsQuery(userId), token);
        return result;
    }
}