using MassTransit;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Services;
using Pilot.Messenger.Interfaces;
using Pilot.Messenger.Models;
using Pilot.SqrsControllerLibrary.RabbitMqMessages;

namespace Pilot.Messenger.Consumers.FromAnotherService;

public class AppliedStatusConsumer(
    ILogger<AppliedStatusConsumer> logger,
    IChatRepository repository,
    IModelService modelService,
    INotificationService notificationService)
    : IConsumer<UpdateCommandMessage<JobApplicationDto>>
{
    public async Task Consume(ConsumeContext<UpdateCommandMessage<JobApplicationDto>> context)
    {
        logger.LogInformation($"{nameof(JobApplicationDto)} update consume");

        var chat = await repository.GetChatAsync(context.Message.UserId, context.Message.Value.UserId,
            context.CancellationToken);
        
        if (chat == null)
        {
            // TODO пока так, потом надо придумать что-то
            var companyPost = await modelService.GetValueByIdAsync<CompanyPostDto>(context.Message.Value.CompanyPost.Id);
            var post = await modelService.GetValueByIdAsync<PostDto>(companyPost!.Post.Id);
            var company = await modelService.GetValueByIdAsync<CompanyDto>(post!.CompanyId);
            chat = new Chat
            {
                Title = $"{company!.Title} HR",
                Description = "HR Department",
                CreatedBy = context.Message.UserId,
                ChatMembers = [new ChatMember { UserId = context.Message.Value.UserId }, new ChatMember { UserId = context.Message.UserId }]
            };
            
            chat.Messages.Add(new Message
            {
                Text = $"This chat is used to notify the user about the status of his vacancy for the position of '{post.Title}' in the company '{company.Title}'",
                UserId = (int)ChatMemberId.System
            });
            
            await repository.AddValueToContextAsync(chat, context.CancellationToken);
        }
        
        chat.Messages.Add(new Message
        {
            Text =  $"The status of your application has been changed to '{context.Message.Value.Status}'",
            UserId = (int)ChatMemberId.System
        });

        await repository.SaveAsync(context.CancellationToken);

        var message = new InfoMessageDto
        {
            MessagePriority = MessageInfo.Success | MessageInfo.Create,
            EntityType = PilotEnumExtensions.GetModelEnumValue<Message>(),
            EntityId = chat.Messages.First().Id
        };
        
        await notificationService.Notify(context.Message.UserId, message);
    }
}