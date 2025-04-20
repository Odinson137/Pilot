using MediatR;
using Pilot.Api.Commands;
using Pilot.Api.Handlers.BaseHandlers;
using Pilot.Api.Interfaces;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO;
using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.Api.Handlers;

public class UserCommandHandler(IBaseMassTransitService massTransitService, IHttpIdentityService httpService, IToken token) :
    ModelCommandHandler<UserDto>(massTransitService),
    IRequestHandler<UserRegistrationCommand>,
    IRequestHandler<UserAuthorizationCommand, AuthUserDto>
{

    public async Task<AuthUserDto> Handle(UserAuthorizationCommand request, CancellationToken cancellationToken)
    {
        var authUser = await httpService.SendPostMessage<AuthUserRoleDto, AuthorizationUserDto>("Authorization",
            request.UserDto,
            cancellationToken);
        var user = new AuthUserDto(authUser.UserId, token.GenerateToken(authUser.UserId, authUser.Role));
        return user;
    }

    public async Task Handle(UserRegistrationCommand request, CancellationToken cancellationToken)
    {
        await httpService.SendPostMessage("Registration", request.UserDto, cancellationToken);
    }
}