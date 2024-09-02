using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Messenger.Models;
using Pilot.SqrsControllerLibrary.Handlers;

namespace Pilot.Messenger.Handlers;

public class MessengerHandler : ModelQueryHandler<Message, MessageDto>
{
    public MessengerHandler(IBaseReadRepository<Message> repository, ILogger<ModelQueryHandler<Message, MessageDto>> logger) : base(repository, logger)
    {
    }
}