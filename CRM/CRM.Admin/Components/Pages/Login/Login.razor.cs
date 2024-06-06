using CRM.Admin.Auth;
using CRM.Admin.Data;
using CRM.Admin.Extensions;
using CRM.Admin.Requests.AuthRequests;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using MudBlazor;

namespace CRM.Admin.Components.Pages.Login;

public partial class Login
{
    [Inject] IAuthenticationRequest AuthenticationRequest { get; set; }
    [Inject] AuthenticationStateProvider AuthenticationStateProvider  {get; set; }
    [Inject] CustomAuthenticationStateProvider CustomAuthStateProvider { get; set; }
    [Inject] NavigationManager NavigationManager { get; set; }
    [Inject] IJSRuntime JSRuntime { get; set; }

    private string _email;
    private string _password;
    private string _accessToken;
    private string _message;
    private Severity _alertSeverity;
    private bool _shouldSetAuthToken;
    private bool isAdmin = false;

 
    public async Task LogIn()
    {
        var response = await AuthenticationRequest.Login(new LoginModel { Email = _email, Password = _password });

        if (response.AccessToken != null)
        {
            await JSRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", response.AccessToken);
            CustomAuthStateProvider.NotifyUserAuthentication(response.AccessToken);
            
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (user.Identity.IsAuthenticated)
        {
            isAdmin = user.IsInRole("Admin");
        }
            NavigationManager.NavigateTo("/client");
        }
        else
        {
            _message = "Authentication failed";
            _alertSeverity = Severity.Error;
        }
    }

    private void ClearMessage()
    {
        _message = string.Empty;
    }
}