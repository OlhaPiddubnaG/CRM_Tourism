using System.Reflection;
using Carter;
using CRM.Core.Extensions;
using CRM.DataAccess;
using CRM.DataAccess.InitialAdminSettings;
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
using CRM.WebApi.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("AppDbContext"),
        optionsBuilder => optionsBuilder.MigrationsAssembly("CRM.DataAccess")));
builder.Services.AddControllers();

builder.Services.ConfigureJwt(builder.Configuration);
builder.Services.AddJwtConfiguration(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CRM API", Version = "v1" });
});

builder.Services.AddScoped<IRequestHandler<LoginUserCommand, LoginUserResponse>, LoginHandler>();

builder.Services.AddScoped<IRequestHandler<CreateCompanyCommand, CreatedResponse>, CreateCompanyHandler>();
builder.Services.AddScoped<IRequestHandler<UpdateCompanyCommand, Unit>, UpdateCompanyHandler>();
builder.Services.AddScoped<IRequestHandler<DeleteCommand<Company>, Unit>, DeleteCompanyHandler>();
builder.Services.AddScoped<IRequestHandler<GetByIdRequest<CompanyResponse>, CompanyResponse>, GetCompanyByIdHandler>();
builder.Services.AddScoped<IRequestHandler<GetAllRequest<CompanyResponse>, List<CompanyResponse>>, GetAllCompaniesHandler>();

builder.Services.AddScoped<IRequestHandler<CreateUserCompanyAdminCommand, CreatedResponse>, CreateUserCompanyAdminHandler>();
builder.Services.AddScoped<IRequestHandler<CreateUserManagerCommand, CreatedResponse>, CreateUserManagerHandler>();
builder.Services.AddScoped<IRequestHandler<UpdateUserCommand, Unit>, UpdateUserHandler>();
builder.Services.AddScoped<IRequestHandler<DeleteCommand<User>, Unit>, DeleteUserHandler>();
builder.Services.AddScoped<IRequestHandler<GetByIdRequest<UserResponse>, UserResponse>, GetUserByIdHandler>();
builder.Services.AddScoped<IRequestHandler<GetAllRequest<UserResponse>, List<UserResponse>>, GetAllUsersHandler>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

builder.Services.Configure<InitialAdminSettings>(builder.Configuration.GetSection(InitialAdminSettings.Section));
builder.Services.AddHostedService<MigrationsService>();
builder.Services.AddAutoMapper(typeof(CompanyProfile).Assembly);
builder.Services.AddAutoMapper(typeof(UserProfile).Assembly);
builder.Services.AddCarter();

var app = builder.Build();
app.UseCors();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapCarter();
app.Run();

