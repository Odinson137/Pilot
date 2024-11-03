using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Pilot.BlazorClient.Data;

namespace Pilot.BlazorClient.Service;

public class TokenAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ProtectedSessionStorage _protectedSessionStore;
    private readonly ILogger<TokenAuthenticationStateProvider> _logger;

    public TokenAuthenticationStateProvider(ProtectedSessionStorage protectedSessionStore, ILogger<TokenAuthenticationStateProvider> logger)
    {
        _protectedSessionStore = protectedSessionStore;
        _logger = logger;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var tokenResult = await _protectedSessionStore.GetAsync<string>(ClientConstants.Token);
        var token = tokenResult.Success ? tokenResult.Value : null;

        ClaimsPrincipal user;

        if (string.IsNullOrEmpty(token))
        {
            user = new ClaimsPrincipal(new ClaimsIdentity());
        }
        else
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, "AuthenticatedUser") 
            };

            var identity = new ClaimsIdentity(claims, "Bearer");
            user = new ClaimsPrincipal(identity);
        }

        return new AuthenticationState(user);
    }
    
    public async Task MarkUserAsAuthenticated(string token, int userId)
    {
        await _protectedSessionStore.SetAsync(ClientConstants.Token, token);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString()), 
            new(ClaimTypes.Name, "AuthenticatedUser")
        };

        var identity = new ClaimsIdentity(claims, "Bearer");
        var user = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }


    public async Task MarkUserAsLoggedOut()
    {
        await _protectedSessionStore.DeleteAsync(ClientConstants.Token);
        await _protectedSessionStore.DeleteAsync(ClientConstants.User);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))));
    }
    
    public async Task<string?> GetTokenAsync()
    {
        var token = await _protectedSessionStore.GetAsync<string>(ClientConstants.Token);
        return token.Value;
    }
}