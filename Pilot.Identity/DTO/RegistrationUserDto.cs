namespace Pilot.Receiver.DTO;

public class RegistrationUserDto
{
    public required string UserName { get; set; }
    public required string Name { get; set; }
    public required string LastName { get; set; }
    public required string Password { get; set; }
}