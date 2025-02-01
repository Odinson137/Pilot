using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;

namespace Pilot.Capability.Models;

public class Post : BaseModel
{
    [Required] [MaxLength(100)] public string Title { get; set; } = null!;

    [MaxLength(500)] public string? Description { get; set; }

    [Required] public int CompanyId { get; set; }

    public ICollection<Skill> Skills { get; set; } = [];

    public ICollection<CompanyPost> CompanyPosts { get; set; } = [];
}