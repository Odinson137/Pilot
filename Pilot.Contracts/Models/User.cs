using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Pilot.Contracts.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [Key] [MaxLength(50)] public string Id { get; } = ObjectId.GenerateNewId().ToString();
    [Required] [MaxLength(50)] public required string UserName { get; set; } = null!;
    [Required] [MaxLength(50)] public required string Name { get; set; } = null!;
    [Required] [MaxLength(50)] public required string LastName { get; set; } = null!;
    [Required] [MaxLength(100)] public required string Password { get; set; } = null!;
    public ICollection<Message>? Messages { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.Now;
}