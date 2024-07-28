using CRM.Admin.Requests.AuthRequests;
using CRM.Admin.Requests.ClientPrivateDataRequests;
using CRM.Admin.Requests.ClientRequests;
using CRM.Admin.Requests.ClientStatusHistoryRequests;
using CRM.Admin.Requests.CompanyRequests;
using CRM.Admin.Requests.CountryRequests;
using CRM.Admin.Requests.OrderRequests;
using CRM.Admin.Requests.OrderStatusHistoryRequests;
using CRM.Admin.Requests.PassportInfoRequests;
using CRM.Admin.Requests.SuperAdminRequests;
using CRM.Admin.Requests.TouroperatorRequests;
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
        services.AddTransient<IClientPrivateDataRequest, ClientPrivateDataRequest>();
        services.AddTransient<IClientStatusHistoryRequest, ClientStatusHistoryRequest>();
        services.AddTransient<IPassportInfoRequest, PassportInfoRequest>();
        services.AddTransient<IOrderRequest, OrderRequest>();
        services.AddTransient<ITouroperatorRequest, TouroperatorRequest>();
        services.AddTransient<ICountryRequest, CountryRequest>();
        services.AddTransient<IOrderStatusHistoryRequest, OrderStatusHistoryRequest>();
        
        return services;
    }
}