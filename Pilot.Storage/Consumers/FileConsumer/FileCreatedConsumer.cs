using MassTransit;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Contracts.Services;
using Pilot.Contracts.Services.LogService;
using Pilot.SqrsControllerLibrary.RabbitMqMessages;
using Pilot.Storage.Interface;
using File = Pilot.Storage.Models.File;

namespace Pilot.Storage.Consumers.FileConsumer;

public class FileCreatedConsumer(
    ILogger<FileCreatedConsumer> logger,
    IFileService fileService,
    IBaseValidatorService validator,
    IMessageService messageService) : IConsumer<CreateCommandMessage<FileDto>>
{
    public async Task Consume(ConsumeContext<CreateCommandMessage<FileDto>> context)
    {
        logger.LogInformation("File create consume");
        logger.LogClassInfo(context.Message);

        var value = context.Message.Value;
        var userId = context.Message.UserId;

        await validator.ValidateAsync<File, FileDto>(value);

        value.UserUploadedId = userId;

        var fileId = await fileService.UploadFileAsync(value);

        var message = new MessageDto
        {
            Title = "Успешное создание файла!",
            Description = $"Успешное создание сущности '{nameof(File)}'",
            MessagePriority = MessageInfo.Success | MessageInfo.Create,
            EntityType = PilotEnumExtensions.GetModelEnumValue<File>(),
            EntityId = fileId
        };

        await messageService.SendMessageAsync(message);

        logger.LogInformation("File consumed");
    }
}