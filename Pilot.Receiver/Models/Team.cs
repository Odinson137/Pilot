using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Pilot.Receiver.Models;

public class Team
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [Key] [MaxLength(50)] public string Id { get; } = ObjectId.GenerateNewId().ToString();
    [Required] [MaxLength(50)] public required string Name { get; set; }
    [Required] [MaxLength(50)] public required string Description { get; set; }
    public ProjectTask ProjectTask { get; set; }
    public ICollection<CompanyUser> CompanyUsers { get; set; }
}