using CRM.Admin.Data;
using CRM.Admin.Requests.AuthRequests;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Pages.Login;

public partial class ForgotPassword
{
    [Inject] IAuthenticationRequest AuthenticationRequest { get; set; }
    [Inject] NavigationManager NavigationManager { get; set; }
    private string _email;
    private string _message;
    private Severity _alertSeverity;

    private async Task ResetPassword()
    {
        var result = await AuthenticationRequest.ForgotPassword(new ForgotPasswordModel { Email = _email });

        if (result.Success)
        {
            _message = "Перевірте свою електронну пошту для подальших інструкцій.";
            _alertSeverity = Severity.Success;
        }
        else
        {
            _message = "Сталася помилка. Перевірте чи правильно введений Ваш Email.";
            await Task.Delay(5000);
            _alertSeverity = Severity.Error;
        }
    }

    private void ClearMessage()
    {
        _message = string.Empty;
    }
}