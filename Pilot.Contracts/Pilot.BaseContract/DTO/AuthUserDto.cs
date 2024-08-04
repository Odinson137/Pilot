using Pilot.Contracts.Data.Enums;

namespace Pilot.Contracts.DTO;

public record AuthUserDto(int UserId, string Token);
public record AuthUserRoleDto(int UserId, Role Role);
