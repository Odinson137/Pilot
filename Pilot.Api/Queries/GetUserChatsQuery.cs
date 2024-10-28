using MediatR;
using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.Api.Queries;

public record GetUserChatsQuery(int UserId) : IRequest<ICollection<ChatDto>>;
