namespace Pilot.Contracts.RabbitMqMessages.CompanyUser;

public class CompanyUserAdded
{
    public string UserId { get; }
    public string AuthorId { get; }
    public string CompanyId { get; }

    public CompanyUserAdded(string userId, string authorId, string companyId)
    {
        UserId = userId;
        AuthorId = authorId;
        CompanyId = companyId;
    }
}