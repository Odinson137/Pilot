using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Queries;

namespace Pilot.Messenger.Queries;

public record GetUserChatsQuery(int UserId)
    : BaseQuery, IRequest<ICollection<ChatDto>>;

