using MongoDB.Bson;

namespace pilot_api.DTO;

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