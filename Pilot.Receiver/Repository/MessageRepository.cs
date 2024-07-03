using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Pilot.Api.Data.Enums;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.Models;
using Pilot.Receiver.Data;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Repository;

public class MessageRepository(DataContext context, IMapper mapper) : BaseRepository<Message>(context, mapper), IMessage
{
    public async Task SendMessage(string title, string desc, MessagePriority priority)
    {
        // await _messageCollection.InsertOneAsync(new Message
        // {
        //     Title = title,
        //     Description = desc,
        //     MessagePriority = priority,
        // });
    }
}