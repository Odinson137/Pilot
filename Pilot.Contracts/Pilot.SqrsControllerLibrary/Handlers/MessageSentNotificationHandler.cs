using MediatR;
using Pilot.Contracts.Interfaces;
using Pilot.SqrsControllerLibrary.Notifications;

namespace Pilot.SqrsControllerLibrary.Handlers;

public class MessageSentNotificationHandler(IMessageService messageService)
    : INotificationHandler<MessageSentNotification>
{
    public async Task Handle(
        MessageSentNotification notification,
        CancellationToken cancellationToken)
    {
        await messageService.SendInfoMessageAsync(
            notification.Message,
            notification.UserId
        );
    }
}