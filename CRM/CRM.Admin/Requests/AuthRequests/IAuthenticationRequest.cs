using CRM.Admin.Data;

namespace CRM.Admin.Requests.AuthRequests;

public interface IAuthenticationRequest
{
    Task<LoginUser> Login(LoginModel loginModel);
    Task<ResultModel> ForgotPassword(ForgotPasswordModel forgotPasswordModel);
    Task ResetPassword(ResetPasswordModel resetPasswordModel);
}