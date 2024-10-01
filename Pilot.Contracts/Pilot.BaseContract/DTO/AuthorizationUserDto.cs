using Pilot.Contracts.Attributes;
using Pilot.Contracts.Data.Enums;

namespace Pilot.Contracts.DTO;

[FromService(ServiceName.IdentityServer)]
public class AuthorizationUserDto
{
    public required string UserName { get; set; }
    public required string Password { get; set; }
}