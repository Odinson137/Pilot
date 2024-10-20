﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;

namespace Pilot.Contracts.Services.LogService;

public static class MongoDbLoggerService
{
    public static LoggerConfiguration MongoDb(
        this LoggerSinkConfiguration sinkConfiguration,
        MongoConfig mongoConfiguration,
        LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum)
    {
        var mongoClient = new MongoClient(mongoConfiguration.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(mongoConfiguration.DbName);
        var mongoCollection = mongoDatabase.GetCollection<MongoLog>(mongoConfiguration.LogCollection);

        return sinkConfiguration.Sink(new MongoDbSink(mongoCollection), restrictedToMinimumLevel);
    }
}

internal class MongoDbSink : ILogEventSink
{
    private readonly IMongoCollection<MongoLog> _collection;

    public MongoDbSink(IMongoCollection<MongoLog> collection)
    {
        _collection = collection;
    }

    public void Emit(LogEvent logEvent)
    {
        if (logEvent.Exception == null) return;

        var mongoLog = new MongoLog(logEvent.RenderMessage(), logEvent.Exception.Message,
            logEvent.Exception.StackTrace);

        _collection.InsertOne(mongoLog);
    }
}

internal class MongoLog
{
    public MongoLog(string message, string exeption, string? stack)
    {
        Message = message;
        Exeption = exeption;
        Stack = stack;
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public string Message { get; set; }
    public string Exeption { get; set; }
    public string? Stack { get; set; }
    public DateTime DateTime { get; set; } = DateTime.Now;
}