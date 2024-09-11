using CRM.Admin.Requests.AuthRequests;
using CRM.Admin.Requests.ClientPrivateDataRequests;
using CRM.Admin.Requests.ClientRequests;
using CRM.Admin.Requests.ClientStatusHistoryRequests;
using CRM.Admin.Requests.CompanyRequests;
using CRM.Admin.Requests.CountryRequests;
using CRM.Admin.Requests.HotelRequests;
using CRM.Admin.Requests.MealsRequests;
using CRM.Admin.Requests.NumberOfPeopleRequests;
using CRM.Admin.Requests.OrderRequests;
using CRM.Admin.Requests.OrderStatusHistoryRequests;
using CRM.Admin.Requests.PassportInfoRequests;
using CRM.Admin.Requests.PaymentRequests;
using CRM.Admin.Requests.RoomTypeRequests;
using CRM.Admin.Requests.StaysRequests;
using CRM.Admin.Requests.SuperAdminRequests;
using CRM.Admin.Requests.TouroperatorRequests;
using CRM.Admin.Requests.UserRequests;
using CRM.Admin.Requests.UserTasksRequests;

namespace CRM.Admin.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddTransient<IAuthenticationRequest, AuthenticationRequest>();
        services.AddTransient<ISuperAdminRequest, SuperAdminRequest>();
        services.AddTransient<ICompanyRequest, CompanyRequest>();
        services.AddTransient<IUserRequest, UserRequest>();
        services.AddTransient<IUserTasksRequest, UserTasksRequest>();
        services.AddTransient<IClientRequest, ClientRequest>();
        services.AddTransient<IClientPrivateDataRequest, ClientPrivateDataRequest>();
        services.AddTransient<IClientStatusHistoryRequest, ClientStatusHistoryRequest>();
        services.AddTransient<IPassportInfoRequest, PassportInfoRequest>();
        services.AddTransient<IOrderRequest, OrderRequest>();
        services.AddTransient<ITouroperatorRequest, TouroperatorRequest>();
        services.AddTransient<ICountryRequest, CountryRequest>();
        services.AddTransient<IOrderStatusHistoryRequest, OrderStatusHistoryRequest>();
        services.AddTransient<INumberOfPeopleRequest, NumberOfPeopleRequest>();
        services.AddTransient<IStaysRequest, StaysRequest>();
        services.AddTransient<IPaymentRequest, PaymentRequest>();
        services.AddTransient<IMealsRequest, MealsRequest>();
        services.AddTransient<IRoomTypeRequest, RoomTypeRequest>();
        services.AddTransient<IHotelRequest, HotelRequest>();
        
        return services;
    }
}