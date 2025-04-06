using Pilot.Contracts.DTO.ModelDto;
using Pilot.Identity.Interfaces;
using Pilot.Identity.Models;
using Pilot.SqrsControllerLibrary.Handlers;

namespace Pilot.Identity.Handlers;

public class UserQueryHandler(IUser repository, ILogger<UserQueryHandler> logger)
    : ModelQueryHandler<User, UserDto>(repository, logger);