using MediatR;
using Pilot.Api.Commands;
using Pilot.Contracts.DTO;
using Pilot.Contracts.Exception.ProjectExceptions;
using Pilot.Contracts.Services.LogService;

namespace Pilot.Api.Handlers;

public class UserCommandHandler : 
    IRequestHandler<UserRegistrationCommand>,
    IRequestHandler<UserAuthorizationCommand, AuthUserDto>
{
    private readonly HttpClient _httpClient;
    
    public UserCommandHandler(
        IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("IdentityServer");
    }
    
    public async Task Handle(UserRegistrationCommand request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync("Registration", request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            throw new BadRequestException(await response.Content.ReadAsStringAsync(cancellationToken));   
        }
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