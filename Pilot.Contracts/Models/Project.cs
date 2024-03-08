using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Pilot.Api.Data.Enums;

namespace Pilot.Contracts.Models;

public class Project
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [Key] [MaxLength(50)] public string Id { get; } = ObjectId.GenerateNewId().ToString();
    [Required] [MaxLength(100)] public required string Name { get; set; }
    [MaxLength(500)] public string? Description { get; set; }
    public ICollection<Team> Teams { get; set; }
    public ICollection<ProjectTask> ProjectTasks { get; set; }
    public ProjectStatus ProjectStatus { get; set; } 
    public DateTime Timestamp { get; set; } = DateTime.Now; 
}