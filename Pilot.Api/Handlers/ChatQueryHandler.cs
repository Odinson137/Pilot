using MediatR;
using Pilot.Api.Handlers.BaseHandlers;
using Pilot.Api.Queries;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data;
using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.Api.Handlers;

public class ChatHandler(IModelService modelService) : ModelQueryHandler<ChatDto>(modelService), IRequestHandler<GetUserChatsQuery, ICollection<ChatDto>>
{
    private readonly IModelService _modelService1 = modelService;

    public async Task<ICollection<ChatDto>> Handle(GetUserChatsQuery request, CancellationToken cancellationToken)
    {
        var userDto = await _modelService1.GetValuesAsync<ChatDto>($"{Urls.UserChats}/{request.UserId}", new BaseFilter(), cancellationToken);
        return userDto;
    }
}