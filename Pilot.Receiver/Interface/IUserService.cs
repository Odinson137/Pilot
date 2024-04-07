using Pilot.Receiver.DTO;

namespace Pilot.Receiver.Interface;

public interface IUserService
{
    public Task<UserDto> GetUserByIdAsync(string userId, CancellationToken cancellationToken);
}