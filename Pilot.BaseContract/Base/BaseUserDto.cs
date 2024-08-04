using System.ComponentModel.DataAnnotations;

namespace Pilot.Contracts.Base;

public class BaseUserDto : BaseDto, IBaseUser
{
    [Required] [MaxLength(50)] public required string UserName { get; set; } = null!;
    [Required] [MaxLength(50)] public required string Name { get; set; } = null!;
    [Required] [MaxLength(50)] public required string LastName { get; set; } = null!;
}