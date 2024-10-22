using Blazored.SessionStorage;
using CRM.Admin.Auth;
using CRM.Admin.Data.AuthModel;
using CRM.Admin.Requests.AuthRequests;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace CRM.Admin.Components.Pages.Login;

public partial class Login
{
    [Inject] private IAuthenticationRequest AuthenticationRequest { get; set; } = null!;
    [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
    [Inject] private ISessionStorageService SessionStorage  { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    
    private string _email = null!;
    private string _password = null!;
    private string _accessToken = null!;
    private string _message = null!;
    private Severity _alertSeverity;

    public async Task LogIn()
    {
        var authResponse = await AuthenticationRequest.Login(new LoginModel { Email = _email, Password = _password });
        if (authResponse.AccessToken != null)
        {
            await SessionStorage.SetItemAsync("token", authResponse.AccessToken);
            ((CustomAuthenticationStateProvider)AuthenticationStateProvider).AuthenticateUser(authResponse.AccessToken!);
            NavigationManager.NavigateTo("/home");
        }
        else
        {
            _message = "Authentication failed";
            _alertSeverity = Severity.Error;
        }
    }
}