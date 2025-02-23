using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.CompanyConsumer;

public class CompanyCreatedConsumer(
    ILogger<CompanyCreatedConsumer> logger,
    IMediator mediator)
    : BaseCreatedConsumer<Company, CompanyDto>(logger, mediator)
{
    // public override async Task Consume(ConsumeContext<CreateCommandMessage<CompanyDto>> context)
    // {
    //     Logger.LogInformation($"{nameof(Company)} create consume");
    //     Logger.LogClassInfo(context.Message);
    //
    //     await Validator.ValidateAsync<Company, CompanyDto>(context.Message.Value);
    //
    //     var model = Mapper.Map<Company>(context.Message.Value);
    //
    //     await Validator.FillValidateAsync(model);
    //
    //     await Repository.AddValueToContextAsync(model);
    //
    //     var firstEmployee = new CompanyUser
    //     {
    //         Id = context.Message.UserId,
    //         Company = model,
    //     };
    //
    //     await companyUser.AddValueToContextAsync(firstEmployee);
    //
    //     await Repository.SaveAsync();
    //
    //     var message = new InfoMessageDto
    //     {
    //         MessagePriority = MessageInfo.Success | MessageInfo.Create,
    //         EntityType = PilotEnumExtensions.GetModelEnumValue<Company>(),
    //         EntityId = model.Id
    //     };
    //
    //     await MessageService.SendInfoMessageAsync(message, context.Message.UserId);
    // }
}