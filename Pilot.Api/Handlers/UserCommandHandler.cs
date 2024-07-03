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
    
    public UserCommandHandler(
        IHttpIdentityService httpService)
    {
        _httpService = httpService;
    }
    
    public async Task Handle(UserRegistrationCommand request, CancellationToken cancellationToken)
    {
        await _httpService.SendPostMessage("Registration", request.UserDto, cancellationToken);
    }

    public async Task<AuthUserDto> Handle(UserAuthorizationCommand request, CancellationToken cancellationToken)
    {
        return await _httpService.SendPostMessage<AuthUserDto, AuthorizationUserDto>("Authorization", request.UserDto,
            cancellationToken);
    }
}