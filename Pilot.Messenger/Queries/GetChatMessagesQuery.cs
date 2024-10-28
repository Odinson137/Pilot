using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Queries;

namespace Pilot.Messenger.Queries;

public record GetChatMessagesQuery(int ChatId)
    : BaseQuery, IRequest<ICollection<MessageDto>>;

