using AutoMapper;
using MassTransit;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Models;
using Pilot.Contracts.RabbitMqMessages;
using Pilot.Contracts.Services.LogService;
using Pilot.Receiver.Consumers.Base;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Consumers.CompanyConsumer;

public class CompanyUpdatedConsumer(
    ILogger<CompanyUpdatedConsumer> logger,
    ICompany company,
    IMessage message,
    IValidatorService validate,
    IMapper mapper,
    ICompanyUser companyUser)
    : BaseUpdateConsumer<Company, CompanyDto>(logger, company, message, validate, mapper, companyUser)
{
    public override async Task Consume(ConsumeContext<UpdateCommandMessage<CompanyDto>> context)
    {
        Logger.LogInformation($"{nameof(Company)} update consume");
        Logger.LogClassInfo(context.Message);

        await Validator.Validate<Company, CompanyDto>(context.Message.Value, context.Message.UserId);
        
        var model = Mapper.Map<Company>(context.Message.Value);
        
        await Repository.GetContext.AddAsync(model);

        await Repository.SaveAsync();
        
        await Message.SendMessage("Успешное обновление!",
            $"Успешное обновление сущности {nameof(Company)}'",
            MessagePriority.Success | MessagePriority.Update);
    }
}