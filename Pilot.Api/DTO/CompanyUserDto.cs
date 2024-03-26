namespace Pilot.Api.DTO;

public class CompanyUserDto
{
    public string Id { get; }
    public string UserName { get; }

    public string Name { get; }

    public string LastName { get; }
    public DateTime Timestamp { get; }

    public CompanyUserDto(string id, string userName, string name, string lastName, DateTime timestamp)
    {
        Id = id;
        UserName = userName;
        Name = name;
        LastName = lastName;
        Timestamp = timestamp;
    }
}