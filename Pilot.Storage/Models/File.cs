using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;

namespace Pilot.Storage.Models;

public class File : BaseModel
{
    [Required] [MaxLength(50)] public string Name { get; set; } = null!;
    
    [Required] [MaxLength(50)] public string Type { get; set; } = null!;
    
    [Required] public FileFormat Format { get; set; }
    
    [Required] public double Size { get; set; }

    [Required] public int UserUploadedId { get; set; }
}