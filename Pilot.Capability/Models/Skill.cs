using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;

namespace Pilot.Capability.Models;

public class Skill : BaseModel
{
    [Required] [MaxLength(100)] public string Title { get; set; } = null!;
    
    public ICollection<Post> Posts { get; set; } = [];
}