using Pilot.Contracts.Attributes;
using Pilot.Contracts.Data.Enums;

namespace Pilot.Contracts.DTO;

[FromService(ServiceName.IdentityServer)]
public class RegistrationUserDto
{
    public required string UserName { get; set; }
    public required string Name { get; set; }
    public required string LastName { get; set; }
    public required string Password { get; set; }
}