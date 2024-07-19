using MassTransit;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Models;
using Pilot.Contracts.RabbitMqMessages;
using Pilot.Contracts.Services.LogService;
using Pilot.Receiver.Interface;
using IMessage = Pilot.Receiver.Interface.IMessage;

namespace Pilot.Receiver.Consumers;

public class CompanyCreatedConsumer : IConsumer<CreateCommandMessage<CompanyDto>>
{
    private readonly ILogger<CompanyCreatedConsumer> _logger;
    private readonly ICompany _company;
    private readonly IUserService _user;
    private readonly IMessage _message;
    private readonly IValidateService _validator;
    public CompanyCreatedConsumer(ILogger<CompanyCreatedConsumer> logger, ICompany company, IMessage message, IUserService user, IValidateService validate)
    {
        _logger = logger;
        _company = company;
        _message = message;
        _user = user;
        _validator = validate;
    }

    public async Task Consume(ConsumeContext<CreateCommandMessage<CompanyDto>> context)
    {
        _logger.LogInformation("Company create consume");
        _logger.LogClassInfo(context.Message);

        await _validator.Validate<Company, CompanyDto>(context.Message.Value, context.Message.UserId);

        var user = (await _user.GetUserByIdAsync(context.Message.UserId))!;
        
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
        
        await _message.SendMessage("Компания создана!",
            $"Успешное создание компании с названием '{context.Message.Value.Title}'",
            MessagePriority.Success);
    }
}