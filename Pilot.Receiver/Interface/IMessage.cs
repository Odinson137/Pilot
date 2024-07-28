using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.Receiver.Interface;

public interface IMessageService
{
    public Task SendMessage(MessageDto message);
}