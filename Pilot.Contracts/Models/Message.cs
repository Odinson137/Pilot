using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Pilot.Api.Data.Enums;

namespace Pilot.Contracts.Models;

public class Message
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [Key] [MaxLength(50)] public string Id { get; } = ObjectId.GenerateNewId().ToString();
    [Required] [MaxLength(100)] public required string Title { get; set; }
    [MaxLength(500)] public string? Description { get; set; }
    public MessagePriority MessagePriority { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now; 
}