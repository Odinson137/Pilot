using Pilot.Api.DTO;
using Pilot.Receiver.DTO;

namespace Pilot.Api.Interfaces.Interfaces;

public interface IUser
{
    public Task RegistrationAsync(RegistrationUserDto userDto, CancellationToken token);
    public Task<AuthUserDto> AuthorizationAsync(AuthorizationUserDto userDto, CancellationToken token);
}

public record AuthUserDto(string UserId, string Token);
