using AutoMapper;
using MassTransit;
using Pilot.Capability.Consumers.Base;
using Pilot.Capability.Interface;
using Pilot.Capability.Models;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.SqrsControllerLibrary.RabbitMqMessages;
using PilotEnumExtensions = Pilot.Contracts.Services.PilotEnumExtensions;

namespace Pilot.Capability.Consumers.PostConsumer;

public class PostUpdatedConsumer(
    ILogger<PostUpdatedConsumer> logger,
    IPost repository,
    ISkill skillRepository,
    IMessageService message,
    IValidatorService validate,
    IMapper mapper)
    : BaseUpdateConsumer<Post, PostDto>(logger, repository, message, validate, mapper)
{
    public override async Task Consume(ConsumeContext<UpdateCommandMessage<PostDto>> context)
    {
        Logger.LogInformation($"{nameof(PostDto)} update consume");

        var dtoModel = context.Message.Value;

        // await Validator.ValidateAsync<T, TDto>(dtoModel);

        var model = Mapper.Map<Post>(dtoModel);

        var existingModel = await repository.GetPostIncludeSkillsAsync(model.Id, context.CancellationToken);
        if (existingModel == null)
            throw new Exception("Entity not found");

        var entityEntry = repository.GetContext.Entry(existingModel);
        entityEntry.CurrentValues.SetValues(model);

        existingModel.ChangeAt = DateTime.Now;

        var skills = model.Skills.Count > 0
            ? await skillRepository.GetSkillsAsync(model.Skills.Select(c => c.Id).ToArray())
            : [];
        existingModel.Skills = skills;

        await Repository.SaveAsync();

        var message = new InfoMessageDto
        {
            MessagePriority = MessageInfo.Success | MessageInfo.Update,
            EntityType = PilotEnumExtensions.GetModelEnumValue<Post>(),
            EntityId = model.Id
        };

        await Message.SendInfoMessageAsync(message, context.Message.UserId);
    }
}