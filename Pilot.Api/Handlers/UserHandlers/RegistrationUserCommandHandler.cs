using MediatR;
using Pilot.Contracts.Exception.ProjectExceptions;

namespace Pilot.Api.Handlers.UserHandlers;
public record UserRegistrationCommand(string UserName, string Name, string LastName, string Password) : IRequest;

public class UserRegistrationCommandHandler : 
    IRequestHandler<UserRegistrationCommand>
{
    private readonly HttpClient _httpClient;
    
    public UserRegistrationCommandHandler(
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

}