using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Base;
using Pilot.Messenger.Interfaces;
using Pilot.Messenger.Models;

namespace Pilot.Messenger.Repository;

public class MessageRepository(DbContext context, IMapper mapper)
    : BaseRepository<Message>(context, mapper), IMessageRepository
{
}