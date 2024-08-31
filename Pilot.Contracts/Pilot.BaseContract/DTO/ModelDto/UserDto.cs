using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;

namespace Pilot.Contracts.DTO.ModelDto;

[FromService(ServiceName.IdentityServer)]
public class UserDto : BaseDto
{
    [Required] [MaxLength(50)] public string UserName { get; set; } = null!;
    
    [Required] [MaxLength(50)] public string Name { get; set; } = null!;
    
    [Required] [MaxLength(50)] public string LastName { get; set; } = null!;
    
    [MaxLength(100)] public string? Email { get; set; }

    [Required] [MaxLength(100)] public Role Role { get; init; }

    [MaxLength(1000)] public string? Description { get; set; }
    
    [MaxLength(100)] public string? Country { get; set; }
    
    [MaxLength(100)] public string? City { get; set; }
    
    [MaxLength(100)] public string? AvatarUrl { get; set; }
    
    public Gender Gender { get; set; }
    
    public DateTime? Birthday { get; set; }
}