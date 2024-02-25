using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using pilot_api.Data.Enums;

namespace pilot_api.Models;

public class HistoryAction
{
    [Key] [Required] [MaxLength(50)] public required ObjectId Id { get; set; }
    [Required] public ObjectId UserId { get; set; }
    // [Required] public User User { get; set; } = null!;
    [Required] public ObjectId ProjectTaskId { get; set; }
    [Required] public ActionState ActionState { get; set; }
    [Required] public DateTime Timestamp { get; set; } = DateTime.Now;
}