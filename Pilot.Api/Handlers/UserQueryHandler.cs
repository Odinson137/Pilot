using MediatR;
using Pilot.Api.Commands;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.Api.Handlers;

public class UserHandler(IModelService modelService) : IRequestHandler<GetUserQuery, UserDto>
{
    public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var userDto = await modelService.GetValueByIdAsync<UserDto>(request.UserId, cancellationToken);
        return userDto;
    }
}