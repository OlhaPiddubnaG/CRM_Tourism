using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Blazored.SessionStorage;

namespace CRM.Admin.Auth;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ISessionStorageService _sessionStorageService;
    private ClaimsPrincipal _anonymous = new (new ClaimsIdentity());

    public CustomAuthenticationStateProvider(ISessionStorageService sessionStorageService)
    {
        _sessionStorageService = sessionStorageService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _sessionStorageService.GetItemAsync<string>("token");
        if(string.IsNullOrEmpty(token))
        {
            return new AuthenticationState(_anonymous);
        }
        var identity = new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwt");
        var user = new ClaimsPrincipal(identity);
        return await Task.FromResult(new AuthenticationState(user));
    }

    public void AuthenticateUser(string token)
    {
        var identity = new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwt");
        var user = new ClaimsPrincipal(identity);
        var state = new AuthenticationState(user);
        NotifyAuthenticationStateChanged(Task.FromResult(state));
    }
}