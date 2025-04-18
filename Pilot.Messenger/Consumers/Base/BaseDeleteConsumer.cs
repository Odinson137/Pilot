using AutoMapper;
using MassTransit;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Services;
using Pilot.Messenger.Interfaces;
using Pilot.SqrsControllerLibrary.RabbitMqMessages;

namespace Pilot.Messenger.Consumers.Base;

public abstract class BaseDeleteConsumer<T, TDto>(
    ILogger<BaseDeleteConsumer<T, TDto>> logger,
    IBaseRepository<T> repository,
    IBaseValidatorService validate,
    IMapper mapper,
    INotificationService notificationService)
    : IConsumer<DeleteCommandMessage<TDto>>
    where T : BaseModel
    where TDto : BaseDto
{
    protected readonly ILogger<BaseDeleteConsumer<T, TDto>> Logger = logger;
    protected readonly IMapper Mapper = mapper;
    protected readonly IBaseRepository<T> Repository = repository;
    protected readonly IBaseValidatorService Validator = validate;
    protected readonly INotificationService NotificationService = notificationService;

    public virtual async Task Consume(ConsumeContext<DeleteCommandMessage<TDto>> context)
    {
        Logger.LogInformation($"{typeof(T).Name} delete consume");
        
        var model = await Validator.DeleteValidateAsync<T>(context.Message.Value, context.CancellationToken);
        
        Repository.Delete(model);
        await Repository.SaveAsync(context.CancellationToken);
        
        var message = new InfoMessageDto
        {
            MessagePriority = MessageInfo.Success | MessageInfo.Delete,
            EntityType = PilotEnumExtensions.GetModelEnumValue<T>()
        };
            
        await NotificationService.Notify(context.Message.UserId, message);
    }
}