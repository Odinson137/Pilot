using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace pilot_api.Models;

public class User
{
    [Key] [Required] [MaxLength(50)] public ObjectId Id { get; set; }
    [Required] [MaxLength(50)] public string UserName { get; set; } = null!;
    [Required] [MaxLength(50)] public string Name { get; set; } = null!;
    [Required] [MaxLength(50)] public string LastName { get; set; } = null!;
    [Required] [MaxLength(100)] public string Password { get; set; } = null!;
    public ObjectId? CompanyId { get; set; }
    public ObjectId? ProjectTaskId { get; set; }
    public ObjectId? TeamId { get; set; }
    // public ICollection<ProjectTask>? Tasks { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;
}