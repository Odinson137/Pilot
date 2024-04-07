using MassTransit;
using Pilot.Api.Data.Enums;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.RabbitMqMessages.CompanyUser;
using Pilot.Contracts.RabbitMqMessages.Message;
using Pilot.Contracts.Services.LogService;
using Pilot.Receiver.DTO;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Consumers;

public class CompanyUserCreatedConsumer : IConsumer<CompanyUserAdded>
{
    private readonly ILogger<CompanyUserCreatedConsumer> _logger;
    private readonly IUserService _userService;
    private readonly ICompanyUser _companyUser;
    private readonly IMessage _message;
    
    public CompanyUserCreatedConsumer(
        ILogger<CompanyUserCreatedConsumer> logger, 
        IUserService userService, 
        ICompanyUser companyUser, 
        IMessage message)
    {
        _logger = logger;
        _userService = userService;
        _companyUser = companyUser;
        _message = message;
    }

    public async Task Consume(ConsumeContext<CompanyUserAdded> context)
    {
        _logger.LogInformation("Company user create consume");
        _logger.LogClassInfo(context);

        var user = await _userService.GetUserByIdAsync(context.Message.UserId, default);

        var author = 
            await _companyUser.GetCompanyUserAsync(context.Message.CompanyId, context.Message.AuthorId);

        if (author.CompanyRole is not (CompanyUserRole.Employer or CompanyUserRole.Owner))
        {
            _logger.LogInformation("Company user has no need rights");
            await _message.SendMessage(new Message
                {
                    Title = "Ошибка в добавление пользователя",
                    Description = $"Ошибка при попытке добавить пользователя с именем '{user.UserName}'",
                    UserId = author.Id,
                    MessagePriority = MessagePriority.Error
                });
            
            return;
        }
        
        _logger.LogInformation("Company user create consumed");
        await _message.SendMessageList(new List<Message>()
        {
            new()
            {
                Title = "Пользователь успешно добавился",
                Description = $"Пользователь '{user.UserName}' успешно добавляется в вашу компанию",
                UserId = author.Id,
                MessagePriority = MessagePriority.Job
            },
            new()
            {
                Title = "Новое место работы",
                Description = $"{user.UserName}, Вы были успешно добавлены в компанию",
                UserId = context.Message.UserId,
                MessagePriority = MessagePriority.Job
            }
        });
    }
}