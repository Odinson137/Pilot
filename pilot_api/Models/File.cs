using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace pilot_api.Models;

public class File
{
    [Key] [Required] [MaxLength(50)] public required ObjectId Id { get; set; }
    [Required] [MaxLength(50)] public required string Url { get; set; }
    [Required] [MaxLength(10)] public required string Type { get; set; }
}