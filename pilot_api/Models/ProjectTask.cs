using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace pilot_api.Models;

public class ProjectTask
{
    [Key] [Required] [MaxLength(50)] public required ObjectId Id { get; set; }
    [Required] [MaxLength(50)] public required string Name { get; set; }
    [MaxLength(500)] public string? Description { get; set; }
    // public ICollection<User>? Users { get; set; }
    public ObjectId? FileId { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;
}