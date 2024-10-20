using AutoMapper;
using MassTransit;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Services;
using Pilot.Contracts.Services.LogService;
using Pilot.Messenger.Interfaces;
using Pilot.SqrsControllerLibrary.RabbitMqMessages;

namespace Pilot.Messenger.Consumers.Base;

public abstract class BaseCreateConsumer<T, TDto> : IConsumer<CreateCommandMessage<TDto>>
    where T : BaseModel
    where TDto : BaseDto
{
    protected readonly IBaseValidatorService Validator;
    protected readonly ILogger<BaseCreateConsumer<T, TDto>> Logger;
    protected readonly IBaseRepository<T> Repository;
    protected readonly IMapper Mapper;
    protected readonly INotificationService NotificationService;

    protected BaseCreateConsumer(
        ILogger<BaseCreateConsumer<T, TDto>> logger,
        IBaseRepository<T> repository,
        IBaseValidatorService validatorService,
        IMapper mapper,
        INotificationService notificationService)
    {
        Validator = validatorService;
        Logger = logger;
        Repository = repository;
        Mapper = mapper;
        NotificationService = notificationService;
    }

    public virtual async Task Consume(ConsumeContext<CreateCommandMessage<TDto>> context)
    {
        Logger.LogInformation($"{typeof(T).Name} create consume");
        Logger.LogClassInfo(context.Message);

        await Validator.ValidateAsync<T, TDto>(context.Message.Value);

        var model = Mapper.Map<T>(context.Message.Value);

        await Validator.FillValidateAsync(model);

        if (model is IAddUser addingUserModel)
            addingUserModel.AddUser(context.Message.UserId);
        
        await Repository.AddValueToContextAsync(model);

        await Repository.SaveAsync();

        var message = new InfoMessageDto
        {
            Title = "Успешное создание!",
            Description = $"Успешное создание сущности '{typeof(T).Name}'",
            MessagePriority = MessageInfo.Success | MessageInfo.Create,
            EntityType = PilotEnumExtensions.GetModelEnumValue<T>(),
            EntityId = model.Id
        };
        
        await NotificationService.Notify(context.Message.UserId, message);
    }
}