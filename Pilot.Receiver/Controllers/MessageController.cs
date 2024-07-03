using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Models;
using Pilot.Receiver.Interface;
using File = Pilot.Contracts.Models.File;

namespace Pilot.Receiver.Controllers;

public class MessageController(IMessage repository, ILogger<MessageController> logger) : BaseSelectController<Message, MessageDto>(repository, logger);