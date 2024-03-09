using MongoDB.Driver;
using Pilot.Api.Data.Enums;
using Pilot.Contracts.Data;
using Pilot.Contracts.Models;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Repository;

public class MessageRepository : IMessage
{
    private readonly IMongoCollection<Message> _messageCollection;

    public MessageRepository(IMongoDatabase mongo)
    {
        _messageCollection = mongo.GetCollection<Message>(MongoTable.Message);
    }

    public async Task SendMessage(string title, string desc, MessagePriority priority)
    {
        await _messageCollection.InsertOneAsync(new Message
        {
            Title = title,
            Description = desc,
            MessagePriority = priority,
        });
    }
}