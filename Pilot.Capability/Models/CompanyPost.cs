using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;

namespace Pilot.Capability.Models;

public class CompanyPost : BaseModel
{
    public int? CompanyUserId { get; set; } // если пусто, значит вакансия может быть открыта

    public bool IsOpen { get; set; } = true;
    [Required] public Post Post { get; set; } = null!;
    
    [MaxLength(500)] public string? Description { get; set; }
}