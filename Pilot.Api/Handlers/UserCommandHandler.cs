using MediatR;
using Pilot.Api.Commands;
using Pilot.Api.Services;
using Pilot.Contracts.DTO;

namespace Pilot.Api.Handlers;

public class UserCommandHandler :
    IRequestHandler<UserRegistrationCommand>,
    IRequestHandler<UserAuthorizationCommand, AuthUserDto> 
{
    private readonly IHttpIdentityService _httpService;
    private readonly IToken _token;
    
    public UserCommandHandler(
        IHttpIdentityService httpService, IToken token)
    {
        _httpService = httpService;
        _token = token;
    }
    
    public async Task Handle(UserRegistrationCommand request, CancellationToken cancellationToken)
    {
        await _httpService.SendPostMessage("Registration", request.UserDto, cancellationToken);
    }

    public async Task<AuthUserDto> Handle(UserAuthorizationCommand request, CancellationToken cancellationToken)
    {
        var authUser = await _httpService.SendPostMessage<AuthUserRoleDto, AuthorizationUserDto>("Authorization", request.UserDto,
            cancellationToken);
        var token = _token.GenerateToken(authUser.UserId, authUser.Role);
        var user = new AuthUserDto(authUser.UserId, token);
        return user;
    }
}