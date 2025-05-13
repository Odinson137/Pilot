using MassTransit;
using Pilot.Capability.Consumers.Base;
using Pilot.Capability.Interface;
using Pilot.Capability.Models;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Contracts.Services;
using Pilot.SqrsControllerLibrary.RabbitMqMessages;

namespace Pilot.Capability.Consumers.SkillConsumer;

public class SkillDeletedConsumer(
    ILogger<SkillDeletedConsumer> logger,
    ISkill repository,
    IMessageService message,
    IValidatorService validate)
    : BaseDeleteConsumer<Skill, SkillDto>(logger, repository, message, validate)
{
    // public override async Task Consume(ConsumeContext<DeleteCommandMessage<SkillDto>> context)
    // {
    //     Logger.LogInformation($"{typeof(Skill).Name} delete consume");
    //     
    //     var model = await Validator.DeleteValidateAsync<Skill>(context.Message.Value, context.CancellationToken);
    //     
    //     Repository.Delete(model);
    //     await Repository.SaveAsync(context.CancellationToken);
    //     
    //     var message = new InfoMessageDto
    //     {
    //         MessagePriority = MessageInfo.Success | MessageInfo.Delete,
    //         EntityType = PilotEnumExtensions.GetModelEnumValue<Skill>()
    //     };
    //     
    //     await MessageService.SendInfoMessageAsync(message, context.Message.UserId);
    // }
}