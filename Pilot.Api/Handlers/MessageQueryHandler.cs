using MediatR;
using Pilot.Api.Handlers.BaseHandlers;
using Pilot.Api.Queries;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data;
using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.Api.Handlers;

public class MessageHandler(IModelService modelService) : ModelQueryHandler<MessageDto>(modelService), IRequestHandler<GetChatMessagesQuery, ICollection<MessageDto>>
{
    private readonly IModelService _modelService1 = modelService;

    public async Task<ICollection<MessageDto>> Handle(GetChatMessagesQuery request, CancellationToken cancellationToken)
    {
        var values = await _modelService1.GetValuesAsync<MessageDto>($"{Urls.ChatMessages}/{request.ChatId}", null, cancellationToken);
        return values;
    }
}