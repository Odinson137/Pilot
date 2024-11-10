using AutoMapper;
using MassTransit;
using Pilot.Capability.Interface;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Contracts.Services;
using Pilot.Contracts.Services.LogService;
using Pilot.SqrsControllerLibrary.RabbitMqMessages;

namespace Pilot.Capability.Consumers.Base;

// TODO потом добавить в каждый сервис поддержку валидации на юзера, пока я сейчас слишком сильно доверяю id токене
public abstract class BaseCreatedConsumer<T, TDto>(
    ILogger<BaseCreatedConsumer<T, TDto>> logger,
    IBaseRepository<T> repository,
    IMessageService message,
    IValidatorService validate,
    IMapper mapper)
    : IConsumer<CreateCommandMessage<TDto>>
    where T : BaseModel
    where TDto : BaseDto
{
    protected readonly ILogger<BaseCreatedConsumer<T, TDto>> Logger = logger;
    protected readonly IMapper Mapper = mapper;
    protected readonly IMessageService MessageService = message;
    protected readonly IBaseRepository<T> Repository = repository;
    protected readonly IValidatorService Validator = validate;

    public virtual async Task Consume(ConsumeContext<CreateCommandMessage<TDto>> context)
    {
        Logger.LogInformation($"{typeof(T).Name} create consume");
        Logger.LogClassInfo(context.Message);

        await Validator.ValidateAsync<T, TDto>(context.Message.Value);

        var model = Mapper.Map<T>(context.Message.Value);

        await Validator.FillValidateAsync(model);

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

        await MessageService.SendMessageAsync(message);
    }
}