using AutoMapper;
using MassTransit;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Contracts.Services;
using Pilot.Contracts.Services.LogService;
using Pilot.SqrsControllerLibrary.RabbitMqMessages;
using Pilot.Storage.Consumers.Base;
using Pilot.Storage.Interface;
using File = Pilot.Storage.Models.File;

namespace Pilot.Storage.Consumers.FIleConsumer;

public class MessageCreatedConsumer(
    ILogger<MessageCreatedConsumer> logger,
    IFileRepository fileRepository,
    IBaseValidatorService validate,
    IMapper mapper,
    IMessageService messageService)
    : BaseCreatedConsumer<File, FileDto>(logger, fileRepository, validate, mapper, messageService)
{
    public override async Task Consume(ConsumeContext<CreateCommandMessage<FileDto>> context)
    {
        Logger.LogInformation("File create consume");
        Logger.LogClassInfo(context.Message);

        var value = context.Message.Value;
        var userId = context.Message.UserId;

        await Validator.ValidateAsync<File, FileDto>(value, userId);

        var model = Mapper.Map<File>(context.Message.Value);

        model.UserUploadedId = userId;

        await Validator.FillValidateAsync(model);

        await Repository.AddValueToContextAsync(model);

        await Repository.SaveAsync();

        var message = new MessageDto
        {
            Title = "Успешное создание файла!",
            Description = $"Успешное создание сущности '{nameof(File)}'",
            MessagePriority = MessageInfo.Success | MessageInfo.Create,
            EntityType = PilotEnumExtensions.GetModelEnumValue<File>(),
            EntityId = model.Id
        };

        await MessageService.SendMessageAsync(message);

        Logger.LogInformation("Message consumed");
    }
}