using MediatR;
using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.SqrsControllerLibrary.Notifications;

public class MessageSentNotification : INotification
{
    public required InfoMessageDto Message { get; set; }
    
    public required int UserId { get; set; }
}