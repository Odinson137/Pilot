using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Pilot.Receiver.Models;

public class Company
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [Key] [MaxLength(50)] public string Id { get; } = ObjectId.GenerateNewId().ToString();
    [Required] [MaxLength(50)] public required string Title { get; set; } = null!;
    [Required] [MaxLength(500)] public string? Description { get; set; }
    public ICollection<Project> Projects { get; set; }
    public ICollection<CompanyUser> CompanyUsers { get; set; }
}

