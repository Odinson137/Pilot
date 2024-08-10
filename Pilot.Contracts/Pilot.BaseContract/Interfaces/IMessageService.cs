using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.Contracts.Interfaces;

public interface IMessageService
{
    public Task SendMessage(MessageDto message);
}