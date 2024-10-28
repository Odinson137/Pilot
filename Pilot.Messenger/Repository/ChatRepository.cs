using System.Collections;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Messenger.Data;
using Pilot.Messenger.Interfaces;
using Pilot.Messenger.Models;

namespace Pilot.Messenger.Repository;

public class ChatRepository(DataContext context, IMapper mapper)
    : BaseRepository<Chat>(context, mapper), IChatRepository
{
    public async Task<ICollection<ChatDto>> GetUserChatsAsync(int userId, CancellationToken token)
    {
        var query = DbSet
            .Where(c => c.CreatedBy == userId || c.ChatMembers.Any(x => x.UserId == userId))
            .OrderBy(c => c.Messages.OrderByDescending(x => x.Id).Select(x => x.Id).FirstOrDefault())
            .ProjectTo<ChatDto>(mapper.ConfigurationProvider);

        return await query.ToListAsync(token);
    }
}