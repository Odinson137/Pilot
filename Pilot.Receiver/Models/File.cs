using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;

namespace Pilot.Receiver.Models;

public class File : BaseModel
{
    [Required] [MaxLength(50)] public string Url { get; set; } = null!;
    [Required] [MaxLength(50)] public string Type { get; set; } = null!;
}