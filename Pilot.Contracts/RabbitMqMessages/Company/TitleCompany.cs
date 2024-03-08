namespace Pilot.Contracts.RabbitMqMessages.Company;

public class TitleCompany
{
    public readonly string UserId;
    public readonly string Title;

    public TitleCompany(string userId, string title)
    {
        UserId = userId;
        Title = title;
    }
}