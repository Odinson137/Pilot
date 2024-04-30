using System.ComponentModel.DataAnnotations;

namespace Pilot.Api.DTO;

public class AuthorizationUserDto
{
    [Required] 
    [MinLength(3)]
    [MaxLength(50)]
    public string UserName { get; set; } = null!;
    [Required] 
    [MinLength(6)]
    [MaxLength(50)]
    public string Password { get; set; } = null!;
}