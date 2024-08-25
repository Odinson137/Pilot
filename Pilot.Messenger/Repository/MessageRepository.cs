using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Messenger.Data;
using Pilot.Messenger.Interfaces;
using Pilot.Messenger.Models;

namespace Pilot.Messenger.Repository;

public class MessageRepository(DataContext context, IMapper mapper)
    : BaseRepository<Message>(context, mapper), IMessageRepository
{
}