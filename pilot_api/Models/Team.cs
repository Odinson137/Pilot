using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace pilot_api.Models;

public class Team
{
    [Key] [Required] [MaxLength(50)] public required ObjectId Id { get; set; }
    [Required] [MaxLength(50)] public required string Name { get; set; }
    [Required] [MaxLength(50)] public required string Description { get; set; }
    [MaxLength(50)] public ObjectId? ProjectId { get; set; }
    // public ICollection<User>? Users { get; set; }
}