using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Handlers;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Handlers;

public class TeamCommandHandler(ITeam repository, IMapper mapper, IBaseValidatorService validateService)
    : ModelCommandHandler<Team, TeamDto>(repository, mapper, validateService);