using Pilot.Contracts.DTO.ModelDto;
using Pilot.Messenger.Interfaces;
using Pilot.Messenger.Models;
using Pilot.SqrsControllerLibrary.Handlers;

namespace Pilot.Messenger.Handlers;

public class InfoMessengerHandler : ModelQueryHandler<InfoMessage, InfoMessageDto>
{
    public InfoMessengerHandler(IInfoMessageRepository repository, ILogger<ModelQueryHandler<InfoMessage, InfoMessageDto>> logger) : base(repository, logger)
    {
    }
}