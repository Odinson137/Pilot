using MassTransit;
using Pilot.LocalUserLibrary.DTO;

namespace Pilot.LocalUserLibrary.Services;

public class UserMassTransitService
{
    IRequestClient<AddUserNoticeRequest> _client;

    public UserMassTransitService(IRequestClient<AddUserNoticeRequest> client)
    {
        _client = client;
    }

    public Task SendRequest(int id)
    {
        return _client.GetResponse<UserNoticeRequestResult>(new {Id = id});
    }
}