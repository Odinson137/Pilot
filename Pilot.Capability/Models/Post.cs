using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;

namespace Pilot.Capability.Models;

public class Post : BaseModel
{
    [Required] [MaxLength(100)] public string Title { get; set; } = null!;

    public ICollection<Skill> Skills { get; set; } = [];
    
    public int? CompanyUserId { get; set; } // если пусто, значит вакансия открыта
}