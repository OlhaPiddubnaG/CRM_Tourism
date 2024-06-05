using CRM.Admin.Data;
using CRM.Admin.Requests.AuthRequests;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using MudBlazor;

namespace CRM.Admin.Components.Pages;

public partial class ResetPassword
{
    [Inject] IAuthenticationRequest AuthenticationRequest { get; set; }
    [Inject] NavigationManager NavigationManager { get; set; }
    [Inject] ISnackbar Snackbar { get; set; }
    private string _newPassword;
    private string _confirmPassword;
    private string _token;

    protected override void OnInitialized()
    {
        var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
        if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("token", out var token))
        {
            _token = token;
        }
    }

    private void ChangePassword()
    {
        if (_newPassword != _confirmPassword)
        {
            Snackbar.Add("Паролі не співпадають", Severity.Error);
        }

        var result = AuthenticationRequest.ResetPassword(new ResetPasswordModel
            { Token = _token, NewPassword = _newPassword, ConfirmPassword = _confirmPassword });

        Snackbar.Add("Password reset successfully", Severity.Success);
        NavigationManager.NavigateTo("/home");
    }
}