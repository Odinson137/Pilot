using System.ComponentModel.DataAnnotations;

namespace Pilot.Api.DTO;

public class RegistrationUserDto
{
    [Required] 
    [MinLength(3)]
    [MaxLength(50)]
    public required string UserName { get; set; }
    [Required] 
    [MinLength(3)]
    [MaxLength(50)]
    public required string Name { get; set; }
    [Required] 
    [MinLength(3)]
    [MaxLength(50)]
    public required string LastName { get; set; }
    [Required] 
    [MinLength(6)]
    [MaxLength(50)]
    public required string Password { get; set; }
}