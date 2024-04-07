namespace Pilot.Contracts.RabbitMqMessages.Company;

public class TitleCompany
{
    public string UserId { get; }
    public string Title { get; }

    public TitleCompany(string userId, string title)
    {
        UserId = userId;
        Title = title;
    }
}