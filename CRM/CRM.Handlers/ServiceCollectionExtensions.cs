using CRM.Domain.Commands;
using CRM.Domain.Commands.Authentication;
using CRM.Domain.Commands.Company;
using CRM.Domain.Commands.User;
using CRM.Domain.Entities;
using CRM.Domain.Requests;
using CRM.Domain.Responses;
using CRM.Domain.Responses.Authentication;
using CRM.Domain.Responses.Company;
using CRM.Domain.Responses.User;
using CRM.Handlers.AuthenticationHandlers;
using CRM.Handlers.CompanyHandlers;
using CRM.Handlers.UserHandlers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CRM.Handlers;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRequestHandlers(this IServiceCollection services)
    {
        services.AddScoped<IRequestHandler<LoginUserCommand, LoginUserResponse>, LoginHandler>();
        services.AddScoped<IRequestHandler<ForgotPasswordCommand, Unit>, ForgotPasswordHandler>();
        services.AddScoped<IRequestHandler<ResetPasswordCommand, Unit>, ResetPasswordHandler> (); 

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

        return services;
    }
}