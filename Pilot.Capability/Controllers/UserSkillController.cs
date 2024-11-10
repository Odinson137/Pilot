using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pilot.Capability.Queries;
using Pilot.Contracts.Data;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Controller;

namespace Pilot.Capability.Controllers;

public class UserSkillController(IMediator mediator) : PilotReadOnlyController<UserSkillDto>(mediator)
{
    [HttpGet(Urls.UserSkills + "/{userId:int}")]
    [ProducesResponseType(200)]
    public async Task<ICollection<UserSkillDto>> GetUserSkills(int userId, CancellationToken token)
    {
        var result = await Mediator.Send(new GetUserSkillQuery(userId), token);
        return result;
    }
}