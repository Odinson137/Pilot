using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Validation.ValidationAttributes;

namespace Pilot.Contracts.Base;

public class BaseUserDto : BaseDto, IBaseUser
{
    [Required] [MaxLength(50)] [CheckNameExist] public required string UserName { get; set; } = null!;
    [Required] [MaxLength(50)] public required string Name { get; set; } = null!;
    [Required] [MaxLength(50)] public required string LastName { get; set; } = null!;
}