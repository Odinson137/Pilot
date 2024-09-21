using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;

namespace Pilot.Capability.Models;

public class Post : BaseModel
{
    [Required] [MaxLength(100)] public string Title { get; set; } = null!;

    [Required] [Range(1, int.MaxValue)] public int CompanyId { get; set; }

    public ICollection<Skill> Skills { get; set; } = [];
}