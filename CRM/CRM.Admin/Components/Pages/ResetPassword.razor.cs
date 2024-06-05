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
    private string _newPassword;
    private string _confirmPassword;
    private string _token;
    private string _message;
    private Severity _alertSeverity;

    protected override void OnInitialized()
    {
        var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
        if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("token", out var token))
        {
            _token = token;
        }
    }

    private async Task ChangePassword()
    {
        if (_newPassword != _confirmPassword)
        {
            _message = "Паролі не співпадають";
            await Task.Delay(5000);
        }

        var result = await AuthenticationRequest.ResetPassword(new ResetPasswordModel
            { Token = _token, NewPassword = _newPassword, ConfirmPassword = _confirmPassword });
        if (result.Success)
        {
            _message = "Пароль змінено успішно";
            _alertSeverity = Severity.Success;
            NavigationManager.NavigateTo("/");
        }
        else
        {
            _message = result.Message ?? "Сталася помилка при спробі скидання пароля. Спробуйте ще раз.";
            _alertSeverity = Severity.Error;
        }
    }

    private void ClearMessage()
    {
        _message = string.Empty;
    }
}