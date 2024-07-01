using CRM.Admin.Data;
using CRM.Admin.Data.AuthModel;

namespace CRM.Admin.Requests.AuthRequests;

public interface IAuthenticationRequest
{
    Task<LoginUser> Login(LoginModel loginModel);
    Task<ResultModel> ForgotPassword(ForgotPasswordModel forgotPasswordModel);
    Task<ResultModel> ResetPassword(ResetPasswordModel resetPasswordModel);
}