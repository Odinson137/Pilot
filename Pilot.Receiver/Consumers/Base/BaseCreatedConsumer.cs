using AutoMapper;
using MassTransit;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.RabbitMqMessages;
using Pilot.Contracts.Services.LogService;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Consumers.Base;

public abstract class BaseCreatedConsumer<T, TDto>(
    ILogger<BaseCreatedConsumer<T, TDto>> logger,
    IBaseRepository<T> repository,
    IMessage message,
    IValidateService validate,
    IMapper mapper,
    ICompanyUser companyUser)
    : IConsumer<CreateCommandMessage<TDto>>
    where T : BaseModel
    where TDto : BaseDto
{
    protected readonly ILogger<BaseCreatedConsumer<T, TDto>> Logger = logger;
    protected readonly IBaseRepository<T> Repository = repository;
    protected  readonly ICompanyUser CompanyUser = companyUser;
    protected  readonly IMessage Message = message;
    protected readonly IValidateService Validator = validate;
    protected  readonly IMapper Mapper = mapper;

    public virtual async Task Consume(ConsumeContext<CreateCommandMessage<TDto>> context)
    {
        Logger.LogInformation($"{typeof(T).Name} create consume");
        Logger.LogClassInfo(context.Message);

        await Validator.Validate<T, TDto>(context.Message.Value, context.Message.UserId);

        var companyUser = await CompanyUser.GetByIdAsync(context.Message.UserId);
        
        var model = Mapper.Map<T>(context.Message.Value);
        
        await Repository.AddNewValueAsync(model);

        await Repository.SaveAsync();
        
        await Message.SendMessage("Успешное создание!",
            $"Успешное создание сущности {typeof(T).Name}'",
            MessagePriority.Success | MessagePriority.Create);
    }
}