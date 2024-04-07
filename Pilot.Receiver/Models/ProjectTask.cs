using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Pilot.Receiver.Models;

public class ProjectTask
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [Key] [MaxLength(50)] public string Id { get; } = ObjectId.GenerateNewId().ToString();
    [Required] [MaxLength(50)] public required string Name { get; set; }
    [MaxLength(500)] public string? Description { get; set; }
    public File? File { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;
}