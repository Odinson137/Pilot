using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Identity.Interfaces;
using Pilot.Identity.Models;
using Pilot.SqrsControllerLibrary.Handlers;

namespace Pilot.Identity.Handlers;

public class UserCommandHandler(IUser repository, IMapper mapper, IBaseValidatorService validateService)
    : ModelCommandHandler<User, UserDto>(repository, mapper, validateService);
