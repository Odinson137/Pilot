using MediatR;
using Pilot.Contracts.DTO;
using Pilot.Contracts.Exception.ProjectExceptions;

namespace Pilot.Api.Handlers.UserHandlers;
public record UserAuthorizationCommand(string UserName, string Password) : IRequest<AuthUserDto>;

public class UserAuthorizationCommandHandler : 
    IRequestHandler<UserAuthorizationCommand, AuthUserDto>
{
    private readonly HttpClient _httpClient;
    
    public UserAuthorizationCommandHandler(
        IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("IdentityServer");
    }
    
    public async Task<AuthUserDto> Handle(UserAuthorizationCommand request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync("Authorization", request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new BadRequestException(result);
        }

        var content = await response.Content.ReadFromJsonAsync<AuthUserDto>(cancellationToken);
        
        return content!;
    }
}