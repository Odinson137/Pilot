using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace pilot_api.Models;

public class Company
{
    [Key] [MaxLength(50)] public ObjectId Id { get; set; }
    [Required] [MaxLength(50)] public required string Title { get; set; } = null!;
    [Required] [MaxLength(500)] public string? Description { get; set; }
    // public ICollection<User>? Employments { get; set; }
    // public ICollection<Project>? Projects { get; set; }
}