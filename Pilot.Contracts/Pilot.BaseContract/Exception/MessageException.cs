using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Services;

namespace Pilot.Contracts.Exception;

public class MessageException(InfoMessageDto message) : System.Exception(message.ToJson());
