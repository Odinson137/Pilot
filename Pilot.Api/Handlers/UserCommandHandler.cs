using MediatR;
using Pilot.Api.Commands;
using Pilot.Contracts.Exception.ProjectExceptions;
using Pilot.Contracts.Services.LogService;

namespace Pilot.Api.Handlers;

public class UserCommandHandler : 
    IRequestHandler<UserRegistrationCommand>,
    IRequestHandler<UserAuthorizationCommand, AuthUserDto>
{
    private const string Url = "https://localhost:7127";
    private readonly ILogger<UserCommandHandler> _logger;
    private readonly HttpClient _httpClient;
    
    public UserCommandHandler(
        ILogger<UserCommandHandler> logger, 
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient("IdentityServer");
    }
    
    public async Task Handle(UserRegistrationCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("User registration handle");
        _logger.LogClassInfo(request);
        
        var response = await _httpClient.PostAsJsonAsync($"Registration", request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            throw new BadRequestException(await response.Content.ReadAsStringAsync(cancellationToken));   
        }
        
        _logger.LogInformation("Successfully registration");
    }

    public async Task<AuthUserDto> Handle(UserAuthorizationCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("User authorization handle");
        _logger.LogClassInfo(request);
        
        var response = await _httpClient.PostAsJsonAsync("Authorization", request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var result = (await response.Content.ReadAsStringAsync(cancellationToken));
            throw new BadRequestException(result);
        }

        var content = await response.Content.ReadFromJsonAsync<AuthUserDto>(cancellationToken);
        
        _logger.LogInformation("Successfully authorization");
        _logger.LogClassInfo(content);

        return content!;
    }
}