using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Messenger.Interfaces;
using Pilot.Messenger.Models;
using Pilot.SqrsControllerLibrary.Handlers;

namespace Pilot.Messenger.Handlers;

public class MessengerHandler : ModelQueryHandler<Message, MessageDto>
{
    public MessengerHandler(IMessageRepository repository, ILogger<ModelQueryHandler<Message, MessageDto>> logger) : base(repository, logger)
    {
    }
}