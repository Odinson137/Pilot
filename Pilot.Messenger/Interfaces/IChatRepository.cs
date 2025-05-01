using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Messenger.Models;

namespace Pilot.Messenger.Interfaces;

public interface IChatRepository : IBaseRepository<Chat>
{
    public Task<ICollection<ChatDto>> GetUserChatsAsync(int userId, CancellationToken token);
    
    public Task<Chat?> GetChatAsync(int chatCreatorId, int userId, CancellationToken token);
}