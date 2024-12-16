using Pilot.BackgroundJob.Interface;
using Pilot.BackgroundJob.Models;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Handlers;

namespace Pilot.BackgroundJob.Handlers;

public class ChatReminderHandler : ModelQueryHandler<ChatReminder, ChatReminderDto>
{
    public ChatReminderHandler(IChatReminder repository, ILogger<ChatReminderHandler> logger) : base(repository, logger)
    {
    }
}