using MediatR;
using Pilot.Contracts.Interfaces;
using Pilot.SqrsControllerLibrary.Notifications;

namespace Pilot.SqrsControllerLibrary.NotificationHandlers;

public class MessageSentNotificationHandler 
    : INotificationHandler<MessageSentNotification>
{
    private readonly IMessageService _messageService;

    public MessageSentNotificationHandler(IMessageService messageService)
    {
        _messageService = messageService;
    }

    public async Task Handle(
        MessageSentNotification notification, 
        CancellationToken cancellationToken)
    {
        await _messageService.SendInfoMessageAsync(
            notification.Message, 
            notification.UserId
        );
    }
}