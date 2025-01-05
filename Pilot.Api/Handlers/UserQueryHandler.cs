using MediatR;
using Pilot.Api.Handlers.BaseHandlers;
using Pilot.Api.Queries;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.Api.Handlers;

public class UserHandler(IModelService modelService) : ModelQueryHandler<UserDto>(modelService), 
    IRequestHandler<GetUserQuery, UserDto>
{
    private readonly IModelService _modelService1 = modelService;

    public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var userDto = await _modelService1.GetValueByIdAsync<UserDto>(request.UserId, cancellationToken);
        return userDto;
    }
}