using CRM.Admin.Data;
using CRM.Admin.Data.AuthModel;
using CRM.Admin.HttpRequests;
using MudBlazor;
using Newtonsoft.Json;

namespace CRM.Admin.Requests.AuthRequests;

public class AuthenticationRequest : IAuthenticationRequest
{
    private readonly IHttpCrmApiRequests _httpCrmApiRequests;
    private readonly ILogger<AuthenticationRequest> _logger;
    private readonly ISnackbar _snackbar;
    private const string RequestUri = "api/Authentication";

    public AuthenticationRequest(IHttpCrmApiRequests httpCrmApiRequests, ILogger<AuthenticationRequest> logger,
        ISnackbar snackbar)
    {
        _httpCrmApiRequests = httpCrmApiRequests;
        _logger = logger;
        _snackbar = snackbar;
    }

    public async Task<LoginUser?> Login(LoginModel loginModel)
    {
        try
        {
            var response = await _httpCrmApiRequests.SendPostRequestAsync($"{RequestUri}/login", loginModel);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                _snackbar.Add("Успішний вхід", Severity.Success);
                return JsonConvert.DeserializeObject<LoginUser>(content);
            }
            else
            {
                _snackbar.Add($"Помилка при вході: {content}", Severity.Error);
                return new LoginUser { Success = false, Message = content };
            }
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
            var response =
                await _httpCrmApiRequests.SendPostRequestAsync($"{RequestUri}/forgotPassword", forgotPasswordModel);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                _snackbar.Add("Лист для скидання пароля успішно відправлено", Severity.Success);
                return new ResultModel
                {
                    Success = true,
                    Message = "Password reset email sent successfully."
                };
            }
            else
            {
                _snackbar.Add($"Помилка при відправці листа для скидання пароля: {content}", Severity.Error);
                return new ResultModel
                {
                    Success = false,
                    Message = "An error occurred while processing your request."
                };
            }
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
            var response =
                await _httpCrmApiRequests.SendPostRequestAsync($"{RequestUri}/resetPassword", resetPasswordModel);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                _snackbar.Add("Пароль успішно змінено", Severity.Success);
                return new ResultModel
                {
                    Success = true,
                    Message = "Password reset successfully."
                };
            }
            else
            {
                _snackbar.Add($"Помилка при зміні пароля: {content}", Severity.Error);
                return new ResultModel
                {
                    Success = false,
                    Message = "An error occurred while resetting your password."
                };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during reset password request");
            _snackbar.Add($"Помилка при зміні пароля: {ex.Message}", Severity.Error);
            throw;
        }
    }
}