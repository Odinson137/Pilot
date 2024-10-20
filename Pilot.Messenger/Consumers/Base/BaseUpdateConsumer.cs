using AutoMapper;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Services;
using Pilot.Messenger.Interfaces;
using Pilot.SqrsControllerLibrary.RabbitMqMessages;

namespace Pilot.Messenger.Consumers.Base;

public abstract class BaseUpdateConsumer<T, TDto>(
    ILogger<BaseUpdateConsumer<T, TDto>> logger,
    IBaseRepository<T> repository,
    IBaseValidatorService validate,
    IMapper mapper,
    INotificationService notificationService)
    : IConsumer<UpdateCommandMessage<TDto>>
    where T : BaseModel
    where TDto : BaseDto
{
    protected readonly ILogger<BaseUpdateConsumer<T, TDto>> Logger = logger;
    protected readonly IMapper Mapper = mapper;
    protected readonly IBaseRepository<T> Repository = repository;
    protected readonly IBaseValidatorService Validator = validate;
    protected readonly INotificationService NotificationService = notificationService;

    public virtual async Task Consume(ConsumeContext<UpdateCommandMessage<TDto>> context)
    {
        Logger.LogInformation($"{typeof(T).Name} update consume");

        var dtoModel = context.Message.Value;

        await Validator.ValidateAsync<T, TDto>(dtoModel);

        var model = Mapper.Map<T>(dtoModel);

        await Validator.FillValidateAsync(model);
        
        model.ChangeAt = DateTime.Now;

        Repository.GetContext.Attach(model);
        Repository.GetContext.Entry(model).State = EntityState.Modified;

        await Repository.SaveAsync();

        var message = new InfoMessageDto
        {
            Title = "Успешное обновление!",
            Description = $"Успешное обновление сущности {typeof(T).Name}'",
            MessagePriority = MessageInfo.Success | MessageInfo.Update,
            EntityType = PilotEnumExtensions.GetModelEnumValue<T>(),
            EntityId = model.Id
        };

        await NotificationService.Notify(context.Message.UserId, message);
    }
}