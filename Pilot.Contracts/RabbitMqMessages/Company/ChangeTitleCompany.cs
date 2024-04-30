namespace Pilot.Contracts.RabbitMqMessages.Company;

public class ChangeTitleCompany
{
    public string UserId { get; }
    public string Id { get; }
    public string Title { get; }

    public ChangeTitleCompany(string userId, string id, string title)
    {
        UserId = userId;
        Id = id;
        Title = title;
    }
}