using AutoMapper;
using MassTransit;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Contracts.RabbitMqMessages;
using Pilot.Contracts.Services;
using Pilot.Contracts.Services.LogService;
using Pilot.Receiver.Interface;
using Pilot.Receiver.Models;
using Pilot.Receiver.Models.ModelHelpers;

namespace Pilot.Receiver.Consumers.Base;

public abstract class BaseCreatedConsumer<T, TDto>(
    ILogger<BaseCreatedConsumer<T, TDto>> logger,
    IBaseRepository<T> repository,
    IMessageService message,
    IBaseValidatorService validate,
    IMapper mapper,
    ICompanyUser companyUser)
    : IConsumer<CreateCommandMessage<TDto>>
    where T : BaseModel
    where TDto : BaseDto
{
    protected readonly ICompanyUser CompanyUser = companyUser;
    protected readonly ILogger<BaseCreatedConsumer<T, TDto>> Logger = logger;
    protected readonly IMapper Mapper = mapper;
    protected readonly IMessageService MessageService = message;
    protected readonly IBaseRepository<T> Repository = repository;
    protected readonly IBaseValidatorService Validator = validate;

    public virtual async Task Consume(ConsumeContext<CreateCommandMessage<TDto>> context)
    {
        Logger.LogInformation($"{typeof(T).Name} create consume");
        Logger.LogClassInfo(context.Message);

        await Validator.ValidateAsync<T, TDto, CompanyUser>(context.Message.Value, context.Message.UserId);

        var model = Mapper.Map<T>(context.Message.Value);

        await Validator.FillValidateAsync(model);

        if (model is IAddCompanyUser addingUserModel)
        {
            var companyUser = await CompanyUser.GetRequiredByIdAsync(context.Message.UserId);
            addingUserModel.AddCompanyUser(companyUser);
        }

        await Repository.AddValueToContextAsync(model);

        await Repository.SaveAsync();

        var message = new MessageDto
        {
            Title = "Успешное создание!",
            Description = $"Успешное создание сущности '{typeof(T).Name}'",
            MessagePriority = MessagePriority.Success | MessagePriority.Create,
            EntityType = PilotEnumExtensions.GetModelEnumValue<T>()
        };

        await MessageService.SendMessage(message);
    }
}