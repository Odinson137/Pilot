using MongoDB.Bson;

namespace Pilot.Api.DTO;

public class CompanyDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }

    public CompanyDto(string id, string title, string? description)
    {
        Id = id;
        Title = title;
        Description = description;
    }
}