using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;

namespace Pilot.Contracts.Models;

public class File : BaseModel
{
    [Required] [MaxLength(50)] public string Url { get; set; }
    [Required] [MaxLength(20)] public string Type { get; set; }
}