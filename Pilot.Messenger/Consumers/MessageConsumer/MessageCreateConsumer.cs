using AutoMapper;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Services.LogService;
using Pilot.Messenger.Consumers.Base;
using Pilot.Messenger.Interfaces;
using Pilot.Messenger.Models;
using Pilot.SqrsControllerLibrary.RabbitMqMessages;

namespace Pilot.Messenger.Consumers.MessageConsumer;

public class MessageCreateConsumer(
    ILogger<MessageCreateConsumer> logger,
    IMessageRepository repository,
    IChatMemberRepository chatMemberRepository,
    IBaseValidatorService validatorService,
    IMapper mapper,
    INotificationService notificationService)
    : BaseCreateConsumer<Message, MessageDto>(logger, repository, validatorService, mapper, notificationService)
{
    public override async Task Consume(ConsumeContext<CreateCommandMessage<MessageDto>> context)
    {
        Logger.LogInformation($"{nameof(Message)} create consume");
        Logger.LogClassInfo(context.Message);

        await Validator.ValidateAsync<Message, MessageDto>(context.Message.Value);

        var model = Mapper.Map<Message>(context.Message.Value);

        await Validator.FillValidateAsync(model);

        model.AddUser(context.Message.UserId);
        
        await Repository.AddValueToContextAsync(model);

        await Repository.SaveAsync();

        var chatMemberIds = await chatMemberRepository.DbSet.Where(c => c.Chat.Id == model.Chat.Id).Select(c => c.UserId).ToListAsync();

        var messageDto = Mapper.Map<MessageDto>(model);
        foreach (var memberId in chatMemberIds)
        {
            logger.LogInformation($"Send message to {memberId}");
            await NotificationService.SendMessage(messageDto, memberId);
        }
    }
}