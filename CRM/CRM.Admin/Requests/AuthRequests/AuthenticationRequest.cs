using CRM.Admin.Data;
using CRM.Admin.HttpRequests;
using Newtonsoft.Json;

namespace CRM.Admin.Requests.AuthRequests;

public class AuthenticationRequest : IAuthenticationRequest
{
    private readonly IHttpCrmApiRequests _httpCrmApiRequests;
    private const string RequestUri = "api/Authentication";

    public AuthenticationRequest(IHttpCrmApiRequests httpCrmApiRequests)
    {
        _httpCrmApiRequests = httpCrmApiRequests;
    }

    public async Task<LoginUser?> Login(LoginModel loginModel)
    {
        var response = await _httpCrmApiRequests.SendPostRequestAsync($"{RequestUri}/login", loginModel);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<LoginUser>(content);
        }
        else
        {
            var content = await response.Content.ReadAsStringAsync();
            return new LoginUser { Success = false, Message = content };
        }
    }

    public async Task<ResultModel> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
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

    public async Task<ResultModel> ResetPassword(ResetPasswordModel resetPasswordModel)
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
}