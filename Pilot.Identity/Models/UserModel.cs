using System.ComponentModel.DataAnnotations;
using Bogus.DataSets;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;

namespace Pilot.Identity.Models;

public class User : BaseModel
{
    [Required] [MaxLength(50)] public string UserName { get; set; } = null!;
    
    [Required] [MaxLength(50)] public string Name { get; set; } = null!;
    
    [Required] [MaxLength(50)] public string LastName { get; set; } = null!;
    
    // TODO пока не нужно, пускай будет просто для вида, потом вдруг захочу сделать сложную регистарцию
    [MaxLength(100)] public string? Email { get; set; }
    
    [Required] [MaxLength(100)] public string Password { get; set; } = null!;
    
    [Required] [MaxLength(100)] public string Salt { get; set; } = null!;
    
    [Required] [MaxLength(100)] public Role Role { get; init; } = Role.User;

    [MaxLength(1000)] public string? Description { get; set; }
    
    [MaxLength(100)] public string? Country { get; set; }
    
    [MaxLength(100)] public string? City { get; set; }
    
    [MaxLength(100)] public string? AvatarUrl { get; set; }
    
    public Name.Gender Gender { get; set; }
    
    public DateTime? Birthday { get; set; }
}