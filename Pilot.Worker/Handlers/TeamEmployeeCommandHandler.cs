using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Handlers;

namespace Pilot.Worker.Handlers;

public class TeamEmployeeCommandHandler(ITeamEmployee repository, IMapper mapper, IBaseValidatorService validateService)
    : ModelCommandHandler<TeamEmployee, TeamEmployeeDto>(repository, mapper, validateService);