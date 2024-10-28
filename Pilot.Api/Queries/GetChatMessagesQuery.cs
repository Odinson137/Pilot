using MediatR;
using Pilot.Api.Controller;
using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.Api.Queries;

public record GetChatMessagesQuery(int ChatId) : IRequest<ICollection<MessageDto>>;
