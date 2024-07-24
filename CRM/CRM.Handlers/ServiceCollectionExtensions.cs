using CRM.Domain.Commands;
using CRM.Domain.Commands.Authentication;
using CRM.Domain.Commands.Client;
using CRM.Domain.Commands.ClientPrivateData;
using CRM.Domain.Commands.ClientStatusHistory;
using CRM.Domain.Commands.Company;
using CRM.Domain.Commands.Country;
using CRM.Domain.Commands.Order;
using CRM.Domain.Commands.PassportInfo;
using CRM.Domain.Commands.Touroperator;
using CRM.Domain.Commands.User;
using CRM.Domain.Entities;
using CRM.Domain.Requests;
using CRM.Domain.Responses;
using CRM.Domain.Responses.Authentication;
using CRM.Domain.Responses.Client;
using CRM.Domain.Responses.ClientPrivateData;
using CRM.Domain.Responses.ClientStatusHistory;
using CRM.Domain.Responses.Company;
using CRM.Domain.Responses.Order;
using CRM.Domain.Responses.PassportInfo;
using CRM.Domain.Responses.Touroperator;
using CRM.Domain.Responses.User;
using CRM.Domain.Responses.Сountry;
using CRM.Handlers.AuthenticationHandlers;
using CRM.Handlers.ClientHandlers;
using CRM.Handlers.ClientPrivateDataHandlers;
using CRM.Handlers.ClientStatusHistoryHandlers;
using CRM.Handlers.CompanyHandlers;
using CRM.Handlers.CountryHandlers;
using CRM.Handlers.OrderHandlers;
using CRM.Handlers.PassportInfoHandlers;
using CRM.Handlers.TouroperatorHandlers;
using CRM.Handlers.UserHandlers;
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

        services.AddScoped<IRequestHandler<CreateClientCommand, CreatedResponse>, CreateClientHandler>();
        services
            .AddScoped<IRequestHandler<CreateClientWithRelatedCommand, ResultBaseResponse>,
                CreateClientWithRelatedHandler>();
        services.AddScoped<IRequestHandler<UpdateClientCommand, ResultBaseResponse>, UpdateClientHandler>();
        services.AddScoped<IRequestHandler<DeleteCommand<Client>, ResultBaseResponse>, DeleteClientHandler>();
        services.AddScoped<IRequestHandler<GetByIdRequest<ClientResponse>, ClientResponse>, GetClientByIdHandler>();
        services
            .AddScoped<IRequestHandler<GetAllRequest<ClientResponse>, List<ClientResponse>>, GetAllClientsHandler>();
        services
            .AddScoped<IRequestHandler<GetFilteredAndSortAllRequest<ClientResponse>, TableData<ClientResponse>>,
                GetSortAllClientsHandler>();

        services.AddScoped<IRequestHandler<CreateClientPrivateDataCommand, CreatedResponse>,
            CreateClientPrivateDataHandler>();
        services
            .AddScoped<IRequestHandler<UpdateClientPrivateDataCommand, ResultBaseResponse>,
                UpdateClientPrivateDataHandler>();
        services
            .AddScoped<IRequestHandler<DeleteCommand<ClientPrivateData>, ResultBaseResponse>,
                DeleteClientPrivateDataHandler>();
        services
            .AddScoped<IRequestHandler<GetByIdRequest<ClientPrivateDataResponse>, ClientPrivateDataResponse>,
                GetClientPrivateDataByIdHandler>();
        services
            .AddScoped<IRequestHandler<GetAllRequest<ClientPrivateDataResponse>, List<ClientPrivateDataResponse>>,
                GetAllClientPrivateDatasHandler>();
        services
            .AddScoped<IRequestHandler<GetByIdRequest<ClientPrivateDataResponse>, ClientPrivateDataResponse>,
                GetClientPrivateDataByClientIdHandler>();

        services
            .AddScoped<IRequestHandler<CreateClientStatusHistoryCommand, CreatedResponse>,
                CreateClientStatusHistoryHandler>();
        services
            .AddScoped<IRequestHandler<GetByIdRequest<ClientStatusHistoryResponse>, ClientStatusHistoryResponse>,
                GetClientStatusHistoryByIdHandler>();
        services
            .AddScoped<IRequestHandler<GetByIdReturnListRequest<ClientStatusHistoryResponse>,
                List<ClientStatusHistoryResponse>>, GetAllClientsStatusHistoryHandler>();

        services.AddScoped<IRequestHandler<CreatePassportInfoCommand, CreatedResponse>, CreatePassportInfoHandler>();
        services.AddScoped<IRequestHandler<UpdatePassportInfoCommand, ResultBaseResponse>, UpdatePassportInfoHandler>();
        services
            .AddScoped<IRequestHandler<DeleteCommand<PassportInfo>, ResultBaseResponse>, DeletePassportInfoHandler>();
        services
            .AddScoped<IRequestHandler<GetByIdRequest<PassportInfoResponse>, PassportInfoResponse>,
                GetPassportInfoByIdHandler>();
        services
            .AddScoped<IRequestHandler<GetAllRequest<PassportInfoResponse>, List<PassportInfoResponse>>,
                GetAllPassportsInfoHandler>();
        services
            .AddScoped<IRequestHandler<GetByIdReturnListRequest<PassportInfoResponse>, List<PassportInfoResponse>>,
                GetByClientPrivateDataIdHandler>();

        services.AddScoped<IRequestHandler<CreateCountryCommand, CreatedResponse>, CreateCountryHandler>();
        services.AddScoped<IRequestHandler<UpdateCountryCommand, ResultBaseResponse>, UpdateCountryHandler>();
        services.AddScoped<IRequestHandler<DeleteCommand<Country>, ResultBaseResponse>, DeleteCountryHandler>();
        services.AddScoped<IRequestHandler<GetByIdRequest<CountryResponse>, CountryResponse>, GetCountryByIdHandler>();
        services
            .AddScoped<IRequestHandler<GetAllRequest<CountryResponse>, List<CountryResponse>>,
                GetAllCountriesHandler>();
        services
            .AddScoped<IRequestHandler<GetByNameRequest<CountryResponse>, CountryResponse>, GetCountryByNameHandler>();

        services.AddScoped<IRequestHandler<CreateOrderCommand, CreatedResponse>, CreateOrderHandler>();
        services.AddScoped<IRequestHandler<UpdateOrderCommand, ResultBaseResponse>, UpdateOrderHandler>();
        services.AddScoped<IRequestHandler<DeleteCommand<Order>, ResultBaseResponse>, DeleteOrderHandler>();
        services.AddScoped<IRequestHandler<GetByIdRequest<OrderResponse>, OrderResponse>, GetOrderByIdHandler>();
        services.AddScoped<IRequestHandler<GetAllRequest<OrderResponse>, List<OrderResponse>>, GetAllOrdersHandler>();

        services.AddScoped<IRequestHandler<CreateTouroperatorCommand, CreatedResponse>, CreateTouroperatorHandler>();
        services.AddScoped<IRequestHandler<UpdateTouroperatorCommand, ResultBaseResponse>, UpdateTouroperatorHandler>();
        services
            .AddScoped<IRequestHandler<DeleteCommand<Touroperator>, ResultBaseResponse>, DeleteTouroperatorHandler>();
        services
            .AddScoped<IRequestHandler<GetByIdRequest<TouroperatorResponse>, TouroperatorResponse>,
                GetTouroperatorByIdHandler>();
        services
            .AddScoped<IRequestHandler<GetAllRequest<TouroperatorResponse>, List<TouroperatorResponse>>,
                GetAllTouroperatorsHandler>();

        return services;
    }
}