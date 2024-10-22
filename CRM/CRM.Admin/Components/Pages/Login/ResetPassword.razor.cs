using CRM.Admin.Data.AuthModel;
using CRM.Admin.Requests.AuthRequests;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using MudBlazor;

namespace CRM.Admin.Components.Pages.Login;

public partial class ResetPassword
{
    [Inject] private IAuthenticationRequest AuthenticationRequest { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    
    private string _newPassword = null!;
    private string _confirmPassword = null!;
    private string _token = null!;
    private string _message = null!;
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
            { Token = _token, NewPassword = _newPassword});
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
}