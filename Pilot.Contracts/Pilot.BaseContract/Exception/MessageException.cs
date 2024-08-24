using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Services;

namespace Pilot.Contracts.Exception;

public class MessageException(MessageDto message) : System.Exception(message.ToJson());
