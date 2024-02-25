using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using pilot_api.Data.Enums;

namespace pilot_api.Models;

public class Project
{
    [Key] [Required] [MaxLength(50)] public required ObjectId Id { get; set; }
    [Required] [MaxLength(100)] public required string Name { get; set; }
    [MaxLength(500)] public ObjectId? Description { get; set; }
    [MaxLength(50)] public ObjectId? CompanyId { get; set; }
    // public ICollection<Team>? Teams { get; set; }
    public ProjectStatus ProjectStatus { get; set; } 
    public DateTime Timestamp { get; set; } = DateTime.Now; 
}