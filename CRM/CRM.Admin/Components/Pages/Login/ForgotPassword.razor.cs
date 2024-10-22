using CRM.Admin.Data.AuthModel;
using CRM.Admin.Requests.AuthRequests;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Pages.Login;

public partial class ForgotPassword
{
    [Inject] private IAuthenticationRequest AuthenticationRequest { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    private string _email = null!;
    private string _message = null!;
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
}