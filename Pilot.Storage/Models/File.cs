using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;

namespace Pilot.Storage.Models;

public class File : BaseModel
{
    [Required] [MaxLength(100)] public string Url { get; set; } = null!;
    
    [Required] [MaxLength(50)] public string Type { get; set; } = null!;
    
    [Required] [MaxLength(50)] public string Name { get; set; } = null!;
    
    [Required] public double Size { get; set; }

    public bool IsPublic { get; set; } = true;

    [Required] public int UserUploadedId { get; set; }
    
    [Required] public ModelType ModelType { get; set; }
}