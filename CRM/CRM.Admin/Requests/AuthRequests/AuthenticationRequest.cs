using CRM.Admin.Data;
using CRM.Admin.Data.AuthModel;
using CRM.Admin.HttpRequests;
using Newtonsoft.Json;

namespace CRM.Admin.Requests.AuthRequests;

public class AuthenticationRequest : IAuthenticationRequest
{
    private readonly IHttpCrmApiRequests _httpCrmApiRequests;
    private readonly ILogger<AuthenticationRequest> _logger;
    private const string RequestUri = "api/Authentication";

    public AuthenticationRequest(IHttpCrmApiRequests httpCrmApiRequests, ILogger<AuthenticationRequest> logger)
    {
        _httpCrmApiRequests = httpCrmApiRequests;
        _logger = logger;
    }

    public async Task<LoginUser?> Login(LoginModel loginModel)
    {
        try
        {
            var response = await _httpCrmApiRequests.SendPostRequestAsync($"{RequestUri}/login", loginModel);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<LoginUser>(content);
            }
            else
            {
                return new LoginUser { Success = false, Message = content };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during login request");
            throw;
        }
    }

    public async Task<ResultModel> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
    {
        try
        {
            var response =
                await _httpCrmApiRequests.SendPostRequestAsync($"{RequestUri}/forgotPassword", forgotPasswordModel);
            await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return new ResultModel
                {
                    Success = true,
                    Message = "Password reset email sent successfully."
                };
            }
            else
            {
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
            throw;
        }
    }

    public async Task<ResultModel> ResetPassword(ResetPasswordModel resetPasswordModel)
    {
        try
        {
            var response =
                await _httpCrmApiRequests.SendPostRequestAsync($"{RequestUri}/resetPassword", resetPasswordModel);
            await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return new ResultModel
                {
                    Success = true,
                    Message = "Password reset successfully."
                };
            }
            else
            {
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
            throw;
        }
    }
}