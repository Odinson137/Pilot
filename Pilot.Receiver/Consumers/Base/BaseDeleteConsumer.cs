using AutoMapper;
using MassTransit;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Contracts.RabbitMqMessages;
using Pilot.Contracts.Services;
using Pilot.Contracts.Services.LogService;

namespace Pilot.Receiver.Consumers.Base;

public abstract class BaseDeleteConsumer<T, TDto>(
    ILogger<BaseDeleteConsumer<T, TDto>> logger,
    IBaseRepository<T> repository,
    IMessageService messageService,
    IBaseValidatorService validate,
    IMapper mapper)
    : IConsumer<DeleteCommandMessage<TDto>>
    where T : BaseModel
    where TDto : BaseDto
{
    protected readonly ILogger<BaseDeleteConsumer<T, TDto>> Logger = logger;
    protected readonly IMapper Mapper = mapper;
    protected readonly IMessageService MessageService = messageService;
    protected readonly IBaseRepository<T> Repository = repository;
    protected readonly IBaseValidatorService Validator = validate;

    public virtual async Task Consume(ConsumeContext<DeleteCommandMessage<TDto>> context)
    {
        Logger.LogInformation($"{typeof(T).Name} delete consume");
        Logger.LogClassInfo(context.Message);

        await Validator.ValidateAsync<T, TDto>(context.Message.Value, context.Message.UserId,
            canDefaultValidate: false);

        var model = Mapper.Map<T>(context.Message.Value);

        await Validator.DeleteValidateAsync(model);

        Repository.DeleteAsync(model);

        await Repository.SaveAsync();

        var message = new MessageDto
        {
            Title = "Успешное удаление!",
            Description = $"Успешное удаление сущности {nameof(T)}'",
            MessagePriority = MessagePriority.Success | MessagePriority.Delete,
            EntityType = PilotEnumExtensions.GetModelEnumValue<T>()
        };

        await MessageService.SendMessage(message);
    }
}