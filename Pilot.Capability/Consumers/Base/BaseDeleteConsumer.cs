﻿using MassTransit;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Contracts.Services;
using Pilot.Contracts.Services.LogService;
using Pilot.SqrsControllerLibrary.RabbitMqMessages;

namespace Pilot.Capability.Consumers.Base;

public abstract class BaseDeleteConsumer<T, TDto>(
    ILogger<BaseDeleteConsumer<T, TDto>> logger,
    IBaseRepository<T> repository,
    IMessageService messageService,
    IBaseValidatorService validate)
    : IConsumer<DeleteCommandMessage<TDto>>
    where T : BaseModel
    where TDto : BaseDto
{
    protected readonly ILogger<BaseDeleteConsumer<T, TDto>> Logger = logger;
    protected readonly IMessageService MessageService = messageService;
    protected readonly IBaseRepository<T> Repository = repository;
    protected readonly IBaseValidatorService Validator = validate;

    // TODO в случае возникновения связанных сущностей будет возникать ошибка. Придумать как её потом обработать
    public virtual async Task Consume(ConsumeContext<DeleteCommandMessage<TDto>> context)
    {
        Logger.LogInformation($"{typeof(T).Name} delete consume");
        Logger.LogClassInfo(context.Message);
        
        var model = await Validator.DeleteValidateAsync<T>(context.Message.Value);
        
        await Repository.FastDeleteAsync(model);

        var message = new InfoMessageDto
        {
            Title = "Успешное удаление!",
            Description = $"Успешное удаление сущности {typeof(T).Name}",
            MessagePriority = MessageInfo.Success | MessageInfo.Delete,
            EntityType = PilotEnumExtensions.GetModelEnumValue<T>()
        };

        await MessageService.SendInfoMessageAsync(message, context.Message.UserId);
    }
}