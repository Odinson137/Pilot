using Pilot.Contracts.Base;

namespace Pilot.Identity.DTO;

public class UserDto : BaseId
{
    public required string UserName { get; set; } = null!;
    public required string Name { get; set; } = null!;
    public required string LastName { get; set; } = null!;
}

public class UpdateUserDto : BaseId
{
    public required string UserName { get; set; } = null!;
    public required string Name { get; set; } = null!;
    public required string LastName { get; set; } = null!;
    public required string OldPassword { get; init; }
    public required string Password { get; init; }

}