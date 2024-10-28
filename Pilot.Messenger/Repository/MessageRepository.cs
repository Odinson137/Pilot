using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Messenger.Data;
using Pilot.Messenger.Interfaces;
using Pilot.Messenger.Models;

namespace Pilot.Messenger.Repository;

public class MessageRepository(DataContext context, IMapper mapper)
    : BaseRepository<Message>(context, mapper), IMessageRepository
{
    public async Task<ICollection<MessageDto>> GetMessagesAsync(int chatId, int page, int skip, CancellationToken cancellationToken)
    {
        var values = await context.Messages
            .Where(c => c.Chat.Id == chatId)
            .ProjectTo<MessageDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return values;
    }
}