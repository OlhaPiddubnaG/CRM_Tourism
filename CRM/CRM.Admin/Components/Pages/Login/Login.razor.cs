using CRM.Admin.Auth;
using CRM.Admin.Data.AuthModel;
using CRM.Admin.Requests.AuthRequests;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace CRM.Admin.Components.Pages.Login;

public partial class Login
{
    [Inject] private IAuthenticationRequest AuthenticationRequest { get; set; } = null!;
    [Inject] private CustomAuthenticationStateProvider CustomAuthStateProvider { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;

    private string _email = null!;
    private string _password = null!;
    private string _accessToken = null!;
    private string _message = null!;
    private Severity _alertSeverity;
    
    public async Task LogIn()
    {
        var response = await AuthenticationRequest.Login(new LoginModel { Email = _email, Password = _password });

        if (response.AccessToken != null)
        {
            await JsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", response.AccessToken);
            CustomAuthStateProvider.NotifyUserAuthentication(response.AccessToken);
            NavigationManager.NavigateTo("/home");
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