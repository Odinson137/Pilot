using MongoDB.Driver;
using Pilot.Contracts.RabbitMqMessages.Message;
using Pilot.Messenger.Data;
using Pilot.Messenger.Interface;

namespace Pilot.Messenger.Repository;

public class MessageMongoRepository : IMessage
{
    private readonly IMongoCollection<Message> _messageCollection;

    public MessageMongoRepository(IMongoDatabase mongoDatabase)
    {
        _messageCollection = mongoDatabase.GetCollection<Message>(MongoTable.Message);
    }
    
    public async Task AddMessageAsync(Message message)
    {
        await _messageCollection.InsertOneAsync(message);
    }
}