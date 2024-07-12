using MassTransit;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Models;
using Pilot.Contracts.RabbitMqMessages;
using Pilot.Contracts.Services.LogService;
using Pilot.Receiver.DTO;
using Pilot.Receiver.Interface;
using IMessage = Pilot.Receiver.Interface.IMessage;

namespace Pilot.Receiver.Consumers;

public class CompanyCreatedConsumer : IConsumer<CreateCommandMessage<CompanyDto>>
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

    public async Task Consume(ConsumeContext<CreateCommandMessage<CompanyDto>> context)
    {
        _logger.LogInformation("Company create consume");
        _logger.LogClassInfo(context.Message);

        // TODO СОЗДАТЬ СВОЙ АТРИБУТ ДЛЯ ЭТОЙ ПРОВЕРКИ
        var companyTitleExist = await _company.CheckCompanyTitleExistAsync(context.Message.Value.Title);

        if (companyTitleExist)
        {
            _logger.LogInformation("Company has already existed");
            await _message.SendMessage("Ошибка в создании",
                $"Ошибка при попытке создать компанию с таким названием '{context.Message.Value.Title}'",
                MessagePriority.Error);
            return;
        }

        var user = await _user.SendGetOneMessage<UserDto>($"api/User/{context.Message.UserId}", default);

        if (user == null)
        {
            _logger.LogInformation("User not found");
            await _message.SendMessage("Вы не найдены",
                $"Вы не можете создать компанию '{context.Message.Value.Title}' потому что вы не существуете в базе",
                MessagePriority.Error);
            return;
        }
        
        await _company.AddNewValueAsync(new Company
        {
            Title = context.Message.Value.Title,
            Description = context.Message.Value.Title,
            CompanyUsers = new List<CompanyUser>
            {
                new()
                {
                    UserName = user.UserName,
                    Name = user.Name,
                    LastName = user.LastName,
                }
            }
        });

        await _company.SaveAsync();
        
        await _message.SendMessage("Create company",
            $"Создание компании '{context.Message.Value.Title}'",
            MessagePriority.Success);
    }
}