using System.Collections;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Messenger.Models;

namespace Pilot.Messenger.Interfaces;

public interface IMessageRepository : IBaseRepository<Message>
{
    public Task<ICollection<MessageDto>> GetMessagesAsync(int chatId, int page, int skip,
        CancellationToken cancellationToken);
}