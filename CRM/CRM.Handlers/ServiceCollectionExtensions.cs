using CRM.Admin.Data;
using CRM.Domain.Commands;
using CRM.Domain.Commands.Authentication;
using CRM.Domain.Commands.Client;
using CRM.Domain.Commands.ClientPrivateData;
using CRM.Domain.Commands.Company;
using CRM.Domain.Commands.Country;
using CRM.Domain.Commands.Hotel;
using CRM.Domain.Commands.Meals;
using CRM.Domain.Commands.NumberOfPeople;
using CRM.Domain.Commands.Order;
using CRM.Domain.Commands.PassportInfo;
using CRM.Domain.Commands.Payment;
using CRM.Domain.Commands.RoomType;
using CRM.Domain.Commands.Stays;
using CRM.Domain.Commands.Touroperator;
using CRM.Domain.Commands.User;
using CRM.Domain.Commands.UserTasks;
using CRM.Domain.Entities;
using CRM.Domain.Requests;
using CRM.Domain.Responses;
using CRM.Domain.Responses.Authentication;
using CRM.Domain.Responses.Client;
using CRM.Domain.Responses.ClientPrivateData;
using CRM.Domain.Responses.ClientStatusHistory;
using CRM.Domain.Responses.Company;
using CRM.Domain.Responses.Hotel;
using CRM.Domain.Responses.Meals;
using CRM.Domain.Responses.NumberOfPeople;
using CRM.Domain.Responses.Order;
using CRM.Domain.Responses.OrderStatusHistory;
using CRM.Domain.Responses.PassportInfo;
using CRM.Domain.Responses.Payment;
using CRM.Domain.Responses.RoomType;
using CRM.Domain.Responses.Stays;
using CRM.Domain.Responses.Touroperator;
using CRM.Domain.Responses.User;
using CRM.Domain.Responses.UserTasks;
using CRM.Domain.Responses.Сountry;
using CRM.Handlers.AuthenticationHandlers;
using CRM.Handlers.ClientHandlers;
using CRM.Handlers.ClientPrivateDataHandlers;
using CRM.Handlers.ClientStatusHistoryHandlers;
using CRM.Handlers.CompanyHandlers;
using CRM.Handlers.CountryHandlers;
using CRM.Handlers.HotelHandlers;
using CRM.Handlers.MealsHandlers;
using CRM.Handlers.NumberOfPeopleHandlers;
using CRM.Handlers.OrderHandlers;
using CRM.Handlers.OrderStatusHistoryHandlers;
using CRM.Handlers.PassportInfoHandlers;
using CRM.Handlers.PaymentHandlers;
using CRM.Handlers.RoomTypeHandlers;
using CRM.Handlers.StaysHandlers;
using CRM.Handlers.TouroperatorHandlers;
using CRM.Handlers.UserHandlers;
using CRM.Handlers.UserTasksHandlers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;

namespace CRM.Handlers;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRequestHandlers(this IServiceCollection services)
    {
        services.AddScoped<IRequestHandler<LoginUserCommand, LoginUserResponse>, LoginHandler>();
        services.AddScoped<IRequestHandler<ForgotPasswordCommand, ResultBaseResponse>, ForgotPasswordHandler>();
        services.AddScoped<IRequestHandler<ResetPasswordCommand, ResultBaseResponse>, ResetPasswordHandler>();
        
        services.AddScoped<IRequestHandler<CreateCompanyCommand, CreatedResponse>, CreateCompanyHandler>();
        services.AddScoped<IRequestHandler<UpdateCompanyCommand, ResultBaseResponse>, UpdateCompanyHandler>();
        services.AddScoped<IRequestHandler<DeleteCommand<Company>, ResultBaseResponse>, DeleteCompanyHandler>();
        services.AddScoped<IRequestHandler<GetByIdRequest<CompanyResponse>, CompanyResponse>, GetCompanyByIdHandler>();
        services.AddScoped<IRequestHandler<GetAllRequest<CompanyResponse>, List<CompanyResponse>>,
            GetAllCompaniesHandler>();
        
        services.AddScoped<IRequestHandler<CreateUserCommand, CreatedResponse>, CreateUserHandler>();
        services.AddScoped<IRequestHandler<UpdateUserCommand, ResultBaseResponse>, UpdateUserHandler>();
        services.AddScoped<IRequestHandler<DeleteCommand<User>, ResultBaseResponse>, DeleteUserHandler>();
        services.AddScoped<IRequestHandler<GetByIdRequest<UserResponse>, UserResponse>, GetUserByIdHandler>();
        services.AddScoped<IRequestHandler<GetAllRequest<UserResponse>, List<UserResponse>>, GetAllUsersHandler>();
        services.AddScoped<IRequestHandler<GetByEmailRequest<ResultModel>, ResultModel>, CheckUserByEmailHandler>();
        
        services.AddScoped<IRequestHandler<CreateUserTasksCommand, CreatedResponse>, CreateUserTasksHandler>();
        services.AddScoped<IRequestHandler<UpdateUserTasksCommand, ResultBaseResponse>, UpdateUserTasksHandler>();
        services.AddScoped<IRequestHandler<DeleteCommand<UserTasks>, ResultBaseResponse>, DeleteUserTasksHandler>();
        services.AddScoped<IRequestHandler<GetByIdRequest<UserTasksResponse>, UserTasksResponse>, GetUserTaskByIdHandler>();
        services.AddScoped<IRequestHandler<GetByIdReturnListRequest<UserTasksResponse>, List<UserTasksResponse>>, GetUserTasksByUserIdHandler>();
        services.AddScoped<IRequestHandler<GetByIdAndDateRequest<UserTasksResponse>, List<UserTasksResponse>>, GetTasksByUserIdAndDateHandler>();
                
        services.AddScoped<IRequestHandler<CreateClientCommand, CreatedResponse>, CreateClientHandler>();
        services.AddScoped<IRequestHandler<CreateClientWithRelatedCommand, ResultBaseResponse>,
                CreateClientWithRelatedHandler>();
        services.AddScoped<IRequestHandler<UpdateClientCommand, ResultBaseResponse>, UpdateClientHandler>();
        services.AddScoped<IRequestHandler<DeleteCommand<Client>, ResultBaseResponse>, DeleteClientHandler>();
        services.AddScoped<IRequestHandler<GetByIdRequest<ClientResponse>, ClientResponse>, GetClientByIdHandler>();
        services.AddScoped<IRequestHandler<GetAllRequest<ClientResponse>, List<ClientResponse>>, GetAllClientsHandler>();
        services.AddScoped<IRequestHandler<GetFilteredAndSortAllRequest<ClientResponse>, TableData<ClientResponse>>,
                GetSortAllClientsHandler>();
        services.AddScoped<IRequestHandler<GetFilteredAllRequest<ClientResponse>, List<ClientResponse>>, GetFilteredClientsHandler>();
        
        services.AddScoped<IRequestHandler<CreateClientPrivateDataCommand, CreatedResponse>,
            CreateClientPrivateDataHandler>();
        services.AddScoped<IRequestHandler<UpdateClientPrivateDataCommand, ResultBaseResponse>,
                UpdateClientPrivateDataHandler>();
        services.AddScoped<IRequestHandler<DeleteCommand<ClientPrivateData>, ResultBaseResponse>,
                DeleteClientPrivateDataHandler>();
        services.AddScoped<IRequestHandler<GetByIdRequest<ClientPrivateDataResponse>, ClientPrivateDataResponse>,
                GetClientPrivateDataByIdHandler>();
        services.AddScoped<IRequestHandler<GetAllRequest<ClientPrivateDataResponse>, List<ClientPrivateDataResponse>>,
                GetAllClientPrivateDatasHandler>();
        services.AddScoped<IRequestHandler<GetByIdRequest<ClientPrivateDataResponse>, ClientPrivateDataResponse>,
                GetClientPrivateDataByClientIdHandler>();
        
        services.AddScoped<IRequestHandler<GetByIdReturnListRequest<ClientStatusHistoryResponse>,
                List<ClientStatusHistoryResponse>>, GetAllClientsStatusHistoryHandler>();
        
        services.AddScoped<IRequestHandler<CreatePassportInfoCommand, CreatedResponse>, CreatePassportInfoHandler>();
        services.AddScoped<IRequestHandler<UpdatePassportInfoCommand, ResultBaseResponse>, UpdatePassportInfoHandler>();
        services.AddScoped<IRequestHandler<DeleteCommand<PassportInfo>, ResultBaseResponse>, DeletePassportInfoHandler>();
        services.AddScoped<IRequestHandler<GetByIdRequest<PassportInfoResponse>, PassportInfoResponse>,
                GetPassportInfoByIdHandler>();
        services.AddScoped<IRequestHandler<GetAllRequest<PassportInfoResponse>, List<PassportInfoResponse>>,
                GetAllPassportsInfoHandler>();
        services.AddScoped<IRequestHandler<GetByIdReturnListRequest<PassportInfoResponse>, List<PassportInfoResponse>>,
                GetByClientPrivateDataIdHandler>();

        services.AddScoped<IRequestHandler<CreateCountryCommand, CreatedResponse>, CreateCountryHandler>();
        services.AddScoped<IRequestHandler<UpdateCountryCommand, ResultBaseResponse>, UpdateCountryHandler>();
        services.AddScoped<IRequestHandler<DeleteCommand<Country>, ResultBaseResponse>, DeleteCountryHandler>();
        services.AddScoped<IRequestHandler<GetByIdRequest<CountryResponse>, CountryResponse>, GetCountryByIdHandler>();
        services.AddScoped<IRequestHandler<GetAllRequest<CountryResponse>, List<CountryResponse>>,
                GetAllCountriesHandler>();
        services.AddScoped<IRequestHandler<GetByNameRequest<CountryResponse>, CountryResponse>, GetCountryByNameHandler>();
        services.AddScoped<IRequestHandler<GetFilteredAllRequest<CountryResponse>, List<CountryResponse>>, GetFilteredCountriesHandler>();

        services.AddScoped<IRequestHandler<CreateOrderCommand, CreatedResponse>, CreateOrderHandler>(); 
        services.AddScoped<IRequestHandler<CreateOrderWithRelatedCommand, ResultBaseResponse>, CreateOrderWithRelatedHandler>();
        services.AddScoped<IRequestHandler<UpdateOrderCommand, ResultBaseResponse>, UpdateOrderHandler>();
        services.AddScoped<IRequestHandler<DeleteCommand<Order>, ResultBaseResponse>, DeleteOrderHandler>();
        services.AddScoped<IRequestHandler<GetByIdRequest<OrderResponse>, OrderResponse>, GetOrderByIdHandler>();
        services.AddScoped<IRequestHandler<GetAllRequest<OrderResponse>, List<OrderResponse>>, GetAllOrdersHandler>();
        services.AddScoped<IRequestHandler<GetFilteredAndSortAllRequest<OrderResponse>, TableData<OrderResponse>>,
                        GetSortAllOrdersHandler>();

        services.AddScoped<IRequestHandler<CreateTouroperatorCommand, CreatedResponse>, CreateTouroperatorHandler>();
        services.AddScoped<IRequestHandler<UpdateTouroperatorCommand, ResultBaseResponse>, UpdateTouroperatorHandler>();
        services.AddScoped<IRequestHandler<DeleteCommand<Touroperator>, ResultBaseResponse>, DeleteTouroperatorHandler>();
        services.AddScoped<IRequestHandler<GetByIdRequest<TouroperatorResponse>, TouroperatorResponse>,
                GetTouroperatorByIdHandler>();
        services.AddScoped<IRequestHandler<GetAllRequest<TouroperatorResponse>, List<TouroperatorResponse>>,
                GetAllTouroperatorsHandler>();
        services.AddScoped<IRequestHandler<GetFilteredAllRequest<TouroperatorResponse>, List<TouroperatorResponse>>, GetFilteredTouroperatorsHandler>();
        
        services.AddScoped<IRequestHandler<GetByIdReturnListRequest<OrderStatusHistoryResponse>, List<OrderStatusHistoryResponse>>,
                GetAllOrderStatusHistoriesHandler>();
        
        services.AddScoped<IRequestHandler<CreateNumberOfPeopleCommand, CreatedResponse>, CreateNumberOfPeopleHandler>();
        services.AddScoped<IRequestHandler<UpdateNumberOfPeopleCommand, ResultBaseResponse>, UpdateNumberOfPeopleHandler>();
        services.AddScoped<IRequestHandler<DeleteCommand<NumberOfPeople>, ResultBaseResponse>, DeleteNumberOfPeopleHandler>();
        services.AddScoped<IRequestHandler<GetByIdRequest<NumberOfPeopleResponse>, NumberOfPeopleResponse>,
                GetNumberOfPeopleByIdHandler>();
        services.AddScoped<IRequestHandler<GetAllRequest<NumberOfPeopleResponse>, List<NumberOfPeopleResponse>>,
                GetAllNumberOfPeopleHandler>();
        
        services.AddScoped<IRequestHandler<CreateStaysCommand, CreatedResponse>, CreateStaysHandler>();
        services.AddScoped<IRequestHandler<UpdateStaysCommand, ResultBaseResponse>, UpdateStaysHandler>();
        services.AddScoped<IRequestHandler<DeleteCommand<Stays>, ResultBaseResponse>, DeleteStaysHandler>();
        services.AddScoped<IRequestHandler<GetByIdRequest<StaysResponse>, StaysResponse>,
                GetStaysByIdHandler>();
        services.AddScoped<IRequestHandler<GetAllRequest<StaysResponse>, List<StaysResponse>>,
                GetAllStaysHandler>();  
        
        services.AddScoped<IRequestHandler<CreateHotelCommand, CreatedResponse>, CreateHotelHandler>();
        services.AddScoped<IRequestHandler<UpdateHotelCommand, ResultBaseResponse>, UpdateHotelHandler>();
        services.AddScoped<IRequestHandler<DeleteCommand<Hotel>, ResultBaseResponse>, DeleteHotelHandler>();
        services.AddScoped<IRequestHandler<GetByIdRequest<HotelResponse>, HotelResponse>,
                GetHotelByIdHandler>();
        services.AddScoped<IRequestHandler<GetAllRequest<HotelResponse>, List<HotelResponse>>,
                GetAllHotelsHandler>();   
        services.AddScoped<IRequestHandler<GetFilteredAllRequest<HotelResponse>, List<HotelResponse>>,
                GetFilteredHotelsHandler>();   
        
        services.AddScoped<IRequestHandler<CreateRoomTypeCommand, CreatedResponse>, CreateRoomTypeHandler>();
        services.AddScoped<IRequestHandler<UpdateRoomTypeCommand, ResultBaseResponse>, UpdateRoomTypeHandler>();
        services.AddScoped<IRequestHandler<DeleteCommand<RoomType>, ResultBaseResponse>, DeleteRoomTypeHandler>();
        services.AddScoped<IRequestHandler<GetAllRequest<RoomTypeResponse>, List<RoomTypeResponse>>,
                GetAllRoomTypesHandler>();
        services.AddScoped<IRequestHandler<GetFilteredAllRequest<RoomTypeResponse>, List<RoomTypeResponse>>,
                GetFilteredRoomTypesHandler>();   
        
        services.AddScoped<IRequestHandler<CreateMealsCommand, CreatedResponse>, CreateMealsHandler>();
        services.AddScoped<IRequestHandler<UpdateMealsCommand, ResultBaseResponse>, UpdateMealsHandler>();
        services.AddScoped<IRequestHandler<DeleteCommand<Meals>, ResultBaseResponse>, DeleteMealsHandler>();
        services.AddScoped<IRequestHandler<GetByIdRequest<MealsResponse>, MealsResponse>,
                GetMealsByIdHandler>();
        services.AddScoped<IRequestHandler<GetAllRequest<MealsResponse>, List<MealsResponse>>,
                GetAllMealsHandler>();
        
        services.AddScoped<IRequestHandler<CreatePaymentCommand, CreatedResponse>, CreatePaymentHandler>();
        services.AddScoped<IRequestHandler<UpdatePaymentCommand, ResultBaseResponse>, UpdatePaymentHandler>();
        services.AddScoped<IRequestHandler<DeleteCommand<Payment>, ResultBaseResponse>, DeletePaymentHandler>();
        services.AddScoped<IRequestHandler<GetByIdRequest<PaymentResponse>, PaymentResponse>,
                GetPaymentByIdHandler>();
        services.AddScoped<IRequestHandler<GetAllRequest<PaymentResponse>, List<PaymentResponse>>,
                GetAllPaymentsHandler>();

        return services;
    }
}