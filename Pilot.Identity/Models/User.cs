using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Pilot.Identity.Data;

namespace Pilot.Identity.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [Key] [MaxLength(50)] public string Id { get; } = ObjectId.GenerateNewId().ToString();
    [Required] [MaxLength(50)] public required string UserName { get; init; }
    [Required] [MaxLength(50)] public required string Name { get; init; }
    [Required] [MaxLength(50)] public required string LastName { get; init; }
    [Required] [MaxLength(100)] public required string Password { get; init; }

    [Required] [MaxLength(100)] public Role Role { get; set; } = Role.User;
    // public ICollection<Message>? Messages { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.Now;
}