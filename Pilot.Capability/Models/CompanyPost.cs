using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;

namespace Pilot.Capability.Models;

public class CompanyPost : BaseModel
{
    [Required] [Range(1, int.MaxValue)] public int CompanyId { get; set; }

    [Required] public Post Post { get; set; } = null!;

    [Required] public ICollection<Skill> Skills { get; set; } = [];
}