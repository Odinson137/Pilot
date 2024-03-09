using MassTransit;
using Pilot.Api.Data.Enums;
using Pilot.Contracts.Models;
using Pilot.Contracts.RabbitMqMessages.Company;
using Pilot.Contracts.Services.LogService;
using Pilot.Receiver.Interface;
using IMessage = Pilot.Receiver.Interface.IMessage;

namespace Pilot.Receiver.Consumers;

public class CompanyCreatedConsumer : IConsumer<TitleCompany>
{
    private readonly ILogger<CompanyCreatedConsumer> _logger;
    private readonly ICompany _company;
    private readonly IUser _user;
    private readonly IMessage _message;
    public CompanyCreatedConsumer(ILogger<CompanyCreatedConsumer> logger, ICompany company, IMessage message, IUser user)
    {
        _logger = logger;
        _company = company;
        _message = message;
        _user = user;
    }

    public async Task Consume(ConsumeContext<TitleCompany> context)
    {
        _logger.LogInformation("Company create consume");
        _logger.LogClassInfo(context.Message);

        var company = await _company.CheckCompanyTitleExistAsync(context.Message.Title);

        if (company == null)
        {
            _logger.LogInformation("Company has already existed");
            await _message.SendMessage("Ошибка в создании",
                $"Ошибка при попытке создать компанию с таким названием '{context.Message.Title}'",
                MessagePriority.Error);
            return;
        }

        var user = await _user.GetUserByIdAsync(context.Message.UserId);

        if (user == null)
        {
            _logger.LogInformation("User not found");
            await _message.SendMessage("Вы не найдены",
                $"Вы не можете создать компанию '{context.Message.Title}' потому что вы не существуете в базе",
                MessagePriority.Error);
            return;
        }
        
        await _company.AddCompanyAsync(new Company
        {
            Title = context.Message.Title,
            Description = context.Message.Title,
            CompanyUsers = new List<CompanyUser>()
            {
                new CompanyUser
                {
                    UserName = user.UserName,
                    Name = user.Name,
                    LastName = user.LastName,
                }
            }
        });
        
        await _message.SendMessage("Create company",
            $"Создание компании '{context.Message.Title}'",
            MessagePriority.Default);
    }
}