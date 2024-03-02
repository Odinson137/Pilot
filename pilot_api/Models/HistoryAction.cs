using System.Collections;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using pilot_api.Data.Enums;

namespace pilot_api.Models;

public class HistoryAction
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [Key] [MaxLength(50)] public string Id { get; } = ObjectId.GenerateNewId().ToString();
    public CompanyUser CompanyUser { get; set; }
    public ProjectTask ProjectTask { get; set; }
    [Required] public ActionState ActionState { get; set; }
    [Required] public DateTime Timestamp { get; set; } = DateTime.Now;
}