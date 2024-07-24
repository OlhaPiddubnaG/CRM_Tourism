using CRM.Admin.Data;
using CRM.Admin.Data.AuthModel;
using CRM.Admin.HttpRequests;
using MudBlazor;

namespace CRM.Admin.Requests.AuthRequests;

public class AuthenticationRequest : IAuthenticationRequest
{
    private readonly IHttpRequests _httpRequests;
    private readonly ILogger<AuthenticationRequest> _logger;
    private readonly ISnackbar _snackbar;
    private const string RequestUri = "api/Authentication";

    public AuthenticationRequest(IHttpRequests httpRequests, ILogger<AuthenticationRequest> logger,
        ISnackbar snackbar)
    {
        _httpRequests = httpRequests;
        _logger = logger;
        _snackbar = snackbar;
    }

    public async Task<LoginUser?> Login(LoginModel loginModel)
    {
        try
        {
            var loginUser = await _httpRequests.SendPostRequestAsync<LoginUser>($"{RequestUri}/login", loginModel);

            if (loginUser != null && loginUser.Success)
            {
                _snackbar.Add("Успішний вхід", Severity.Success);
            }

            return loginUser;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during login request");
            _snackbar.Add($"Помилка при вході: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<ResultModel> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
    {
        try
        {
            var result =
                await _httpRequests.SendPostRequestAsync<ResultModel>($"{RequestUri}/forgotPassword",
                    forgotPasswordModel);

            if (result.Success)
            {
                _snackbar.Add("Лист для скидання пароля успішно відправлено", Severity.Success);
            }
            else
            {
                _snackbar.Add($"Помилка при відправці листа для скидання пароля: {result.Message}", Severity.Error);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during forgot password request");
            _snackbar.Add($"Помилка при відправці листа для скидання пароля: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<ResultModel> ResetPassword(ResetPasswordModel resetPasswordModel)
    {
        try
        {
            var result =
                await _httpRequests.SendPostRequestAsync<ResultModel>($"{RequestUri}/resetPassword",
                    resetPasswordModel);

            if (result.Success)
            {
                _snackbar.Add("Пароль успішно змінено", Severity.Success);
            }
            else
            {
                _snackbar.Add($"Помилка при зміні пароля: {result.Message}", Severity.Error);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during reset password request");
            _snackbar.Add($"Помилка при зміні пароля: {ex.Message}", Severity.Error);
            throw;
        }
    }
}