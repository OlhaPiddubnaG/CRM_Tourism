using CRM.Admin.Auth;
using CRM.Admin.Data;
using CRM.Admin.Extensions;
using CRM.Admin.Requests.AuthRequests;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace CRM.Admin.Components.Pages;

public partial class Login
{
    [Inject] IAuthenticationRequest AuthenticationRequest { get; set; }
    [Inject] CustomAuthenticationStateProvider CustomAuthStateProvider { get; set; }
    [Inject] NavigationManager NavigationManager { get; set; }
    [Inject] IJSRuntime JSRuntime { get; set; }

    private string _email;
    private string _password;
    private bool _shouldSetAuthToken;
    private string _accessToken;
    private string _message;
    private Severity _alertSeverity;

    public async Task LogIn()
    {
        var response = await AuthenticationRequest.Login(new LoginModel { Email = _email, Password = _password });

        if (response.AccessToken != null)
        {
            await JSRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", response.AccessToken);
            CustomAuthStateProvider.NotifyUserAuthentication(response.AccessToken);
            NavigationManager.NavigateTo("/home");
        }
        else
        {
            _message = "Authentication failed";
            _alertSeverity = Severity.Error;
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (_shouldSetAuthToken)
        {
            _shouldSetAuthToken = false;
            if (JSRuntime.IsInvokeable())
            {
                await JSRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", _accessToken);
                CustomAuthStateProvider.NotifyUserAuthentication(_accessToken);
                NavigationManager.NavigateTo("/home");
            }
        }
    }

    private void ClearMessage()
    {
        _message = string.Empty;
    }
}