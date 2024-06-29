using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;

namespace Pilot.Contracts.Models;

public class File : BaseModel
{
    [Required] [MaxLength(50)] public required string Url { get; set; }
    [Required] [MaxLength(10)] public required string Type { get; set; }
}