using CRM.Admin.Requests.AuthRequests;
using CRM.Admin.Requests.ClientRequests;
using CRM.Admin.Requests.CompanyRequests;
using CRM.Admin.Requests.SuperAdminRequests;
using CRM.Admin.Requests.UserRequests;

namespace CRM.Admin.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddTransient<IAuthenticationRequest, AuthenticationRequest>();
        services.AddTransient<ISuperAdminRequest, SuperAdminRequest>();
        services.AddTransient<ICompanyRequest, CompanyRequest>();
        services.AddTransient<IUserRequest, UserRequest>();
        services.AddTransient<IClientRequest, ClientRequest>();
        
        return services;
    }
}