using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Receiver.Interface;
using Pilot.Receiver.Models;

namespace Pilot.Receiver.Controllers;

public class CompanyUserController(ICompanyUser repository, ILogger<CompanyUserController> logger, IMapper mapper)
    : BaseReadOnlyController<CompanyUser, CompanyUserDto>(repository, logger)
{
    [HttpPost]
    [Route("localuser")]
    [Authorize("Admin")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> AddLocalUser(UserDto userDto, CancellationToken token)
    {
        logger.LogInformation("Add local user in Receiver service");
        
        var localUser = mapper.Map<CompanyUser>(userDto);
        
        await repository.AddValueToContextAsync(localUser, token);
        await repository.SaveAsync(token);
        
        return Ok();
    }
}