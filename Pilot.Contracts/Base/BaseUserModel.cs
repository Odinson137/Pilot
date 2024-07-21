using System.ComponentModel.DataAnnotations;

namespace Pilot.Contracts.Base;

public abstract class BaseUserModel : BaseModel, IBaseUser
{
    [Required] [MaxLength(50)] public string UserName { get; set; } = null!;
    [Required] [MaxLength(50)] public string Name { get; set; } = null!;
    [Required] [MaxLength(50)] public string LastName { get; set; } = null!;
}