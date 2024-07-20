using AutoMapper;
using MassTransit;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.RabbitMqMessages;
using Pilot.Contracts.Services.LogService;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Consumers.Base;

public abstract class BaseDeleteConsumer<T, TDto>(
    ILogger<BaseDeleteConsumer<T, TDto>> logger,
    IBaseRepository<T> repository,
    IMessage message,
    IValidatorService validate,
    IMapper mapper,
    ICompanyUser companyUser)
    : IConsumer<DeleteCommandMessage<TDto>>
    where T : BaseModel
    where TDto : BaseDto
{
    protected readonly ILogger<BaseDeleteConsumer<T, TDto>> Logger = logger;
    protected readonly IBaseRepository<T> Repository = repository;
    protected  readonly ICompanyUser CompanyUser = companyUser;
    protected  readonly IMessage Message = message;
    protected readonly IValidatorService Validator = validate;
    protected  readonly IMapper Mapper = mapper;

    public virtual async Task Consume(ConsumeContext<DeleteCommandMessage<TDto>> context)
    {
        Logger.LogInformation($"{typeof(T).Name} delete consume");
        Logger.LogClassInfo(context.Message);

        await Validator.Validate<T, TDto>(context.Message.Value, context.Message.UserId);

        var model = Mapper.Map<T>(context.Message.Value);
        
        Repository.DeleteAsync(model);

        await Repository.SaveAsync();
        
        await Message.SendMessage("Успешное удаление!",
            $"Успешное удаление сущности {typeof(T).Name}'",
            MessagePriority.Success | MessagePriority.Delete);
    }
}