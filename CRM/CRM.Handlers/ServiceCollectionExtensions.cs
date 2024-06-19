using CRM.Domain.Commands;
using CRM.Domain.Commands.Authentication;
using CRM.Domain.Commands.Client;
using CRM.Domain.Commands.ClientPrivateData;
using CRM.Domain.Commands.Company;
using CRM.Domain.Commands.Country;
using CRM.Domain.Commands.PassportInfo;
using CRM.Domain.Commands.User;
using CRM.Domain.Entities;
using CRM.Domain.Requests;
using CRM.Domain.Responses;
using CRM.Domain.Responses.Authentication;
using CRM.Domain.Responses.Client;
using CRM.Domain.Responses.ClientPrivateData;
using CRM.Domain.Responses.Company;
using CRM.Domain.Responses.PassportInfo;
using CRM.Domain.Responses.User;
using CRM.Domain.Responses.Сountry;
using CRM.Handlers.AuthenticationHandlers;
using CRM.Handlers.ClientHandlers;
using CRM.Handlers.ClientPrivateDataHandlers;
using CRM.Handlers.CompanyHandlers;
using CRM.Handlers.CountryHandlers;
using CRM.Handlers.PassportInfoHandlers;
using CRM.Handlers.UserHandlers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CRM.Handlers;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRequestHandlers(this IServiceCollection services)
    {
        services.AddScoped<IRequestHandler<LoginUserCommand, LoginUserResponse>, LoginHandler>();
        services.AddScoped<IRequestHandler<ForgotPasswordCommand,ResultBaseResponse>, ForgotPasswordHandler>();
        services.AddScoped<IRequestHandler<ResetPasswordCommand,ResultBaseResponse>, ResetPasswordHandler> (); 

        services.AddScoped<IRequestHandler<CreateCompanyCommand, CreatedResponse>, CreateCompanyHandler>();
        services.AddScoped<IRequestHandler<UpdateCompanyCommand, Unit>, UpdateCompanyHandler>();
        services.AddScoped<IRequestHandler<DeleteCommand<Company>, Unit>, DeleteCompanyHandler>();
        services.AddScoped<IRequestHandler<GetByIdRequest<CompanyResponse>, CompanyResponse>, GetCompanyByIdHandler>();
        services.AddScoped<IRequestHandler<GetAllRequest<CompanyResponse>, List<CompanyResponse>>,
            GetAllCompaniesHandler>();
        
        services.AddScoped<IRequestHandler<CreateUserCommand, CreatedResponse>, CreateUserHandler>();
        services.AddScoped<IRequestHandler<UpdateUserCommand, Unit>, UpdateUserHandler>();
        services.AddScoped<IRequestHandler<DeleteCommand<User>, Unit>, DeleteUserHandler>();
        services.AddScoped<IRequestHandler<GetByIdRequest<UserResponse>, UserResponse>, GetUserByIdHandler>();
        services.AddScoped<IRequestHandler<GetAllRequest<UserResponse>, List<UserResponse>>, GetAllUsersHandler>();
        
        services.AddScoped<IRequestHandler<CreateClientCommand, CreatedResponse>, CreateClientHandler>();
        services.AddScoped<IRequestHandler<UpdateClientCommand, Unit>, UpdateClientHandler>();
        services.AddScoped<IRequestHandler<DeleteCommand<Client>, Unit>, DeleteClientHandler>();
        services.AddScoped<IRequestHandler<GetByIdRequest<ClientResponse>, ClientResponse>, GetClientByIdHandler>(); 
        services.AddScoped<IRequestHandler<GetAllRequest<ClientResponse>, List<ClientResponse>>, GetAllClientsHandler>();

        services.AddScoped<IRequestHandler<CreateClientPrivateDataCommand, CreatedResponse>,
                CreateClientPrivateDataHandler>(); 
        services.AddScoped<IRequestHandler<UpdateClientPrivateDataCommand, Unit>, UpdateClientPrivateDataHandler>();
        services.AddScoped<IRequestHandler<DeleteCommand<ClientPrivateData>, Unit>, DeleteClientPrivateDataHandler>();
        services.AddScoped<IRequestHandler<GetByIdRequest<ClientPrivateDataResponse>, ClientPrivateDataResponse>, GetClientPrivateDataByIdHandler>(); 
        services.AddScoped<IRequestHandler<GetAllRequest<ClientPrivateDataResponse>, List<ClientPrivateDataResponse>>, GetAllClientPrivateDatasHandler>(); 
        
        services.AddScoped<IRequestHandler<CreatePassportInfoCommand, CreatedResponse>, CreatePassportInfoHandler>();
        services.AddScoped<IRequestHandler<UpdatePassportInfoCommand, Unit>, UpdatePassportInfoHandler>();
        services.AddScoped<IRequestHandler<DeleteCommand<PassportInfo>, Unit>, DeletePassportInfoHandler>();
        services.AddScoped<IRequestHandler<GetByIdRequest<PassportInfoResponse>, PassportInfoResponse>, GetPassportInfoByIdHandler>(); 
        services.AddScoped<IRequestHandler<GetAllRequest<PassportInfoResponse>, List<PassportInfoResponse>>, GetAllPassportsInfoHandler>();
        
        services.AddScoped<IRequestHandler<CreateCountryCommand, CreatedResponse>, CreateCountryHandler>();
        services.AddScoped<IRequestHandler<UpdateCountryCommand, Unit>, UpdateCountryHandler>();
        services.AddScoped<IRequestHandler<DeleteCommand<Country>, Unit>, DeleteCountryHandler>();
        services.AddScoped<IRequestHandler<GetByIdRequest<CountryResponse>, CountryResponse>, GetCountryByIdHandler>(); 
        services.AddScoped<IRequestHandler<GetAllRequest<CountryResponse>, List<CountryResponse>>, GetAllCountriesHandler>(); 
        services.AddScoped<IRequestHandler<GetByNameRequest<CountryResponse>, CountryResponse>, GetCountryByNameHandler> (); 

        return services;
    }
}