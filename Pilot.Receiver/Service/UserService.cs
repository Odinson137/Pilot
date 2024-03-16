using Pilot.Receiver.DTO;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Service;

public class UserService : IUserService
{
    public Task<UserDto?> GetUserByIdAsync(string userId)
    {
        throw new NotImplementedException(); // сделать здесь отправку данных на Identity сервис 

    }
}