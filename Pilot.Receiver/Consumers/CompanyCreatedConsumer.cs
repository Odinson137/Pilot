using MassTransit;
using Pilot.Api.Data.Enums;
using Pilot.Contracts.RabbitMqMessages.Company;
using Pilot.Contracts.RabbitMqMessages.Message;
using Pilot.Contracts.Services.LogService;
using Pilot.Receiver.Interface;
using Pilot.Receiver.Models;
using IMessage = Pilot.Receiver.Interface.IMessage;

namespace Pilot.Receiver.Consumers;

public class CompanyCreatedConsumer : IConsumer<TitleCompany>
{
    private readonly ILogger<CompanyCreatedConsumer> _logger;
    private readonly ICompany _company;
    private readonly IUserService _user;
    private readonly IMessage _message;

    public CompanyCreatedConsumer(ILogger<CompanyCreatedConsumer> logger, ICompany company, IMessage message, IUserService user)
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
            await _message.SendMessage(new Message
                {
                    Title = "Ошибка в создании",
                    Description = $"Ошибка при попытке создать компанию с таким названием '{context.Message.Title}'",
                    UserId = context.Message.UserId,
                    MessagePriority = MessagePriority.Error
                });
            return;
        }

        var user = await _user.GetUserByIdAsync(context.Message.UserId, default);

        await _company.AddCompanyAsync(new Company
        {
            Title = context.Message.Title,
            Description = context.Message.Title,
            CompanyUsers = new List<CompanyUser>()
            {
                new()
                {
                    UserName = user.UserName,
                    Name = user.Name,
                    LastName = user.LastName,
                    Role = user.CompanyRole,
                }
            }
        });
        
        await _message.SendMessage(new Message
            {
                Title = "Create company",
                Description = $"Создание компании '{context.Message.Title}'",
                UserId = context.Message.UserId,
                MessagePriority = MessagePriority.Default
            });
    }
}