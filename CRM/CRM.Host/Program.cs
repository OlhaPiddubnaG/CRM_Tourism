using System.Reflection;
using Carter;
using CRM.Core.Extensions;
using CRM.DataAccess;
using CRM.DataAccess.InitialAdminSettings;
using CRM.Domain.Enums;
using CRM.Handlers;
using CRM.Handlers.Services;
using CRM.Handlers.Services.CurrentUser;
using CRM.Handlers.Services.Email;
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
builder.Services.AddHttpContextAccessor();

builder.Services.ConfigureJwt(builder.Configuration);
builder.Services.AddJwtConfiguration(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CRM API", Version = "v1" });
});
builder.Services.AddRequestHandlers();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole(RoleType.Admin.ToString()));
    options.AddPolicy("CompanyAdmin", policy => policy.RequireRole(RoleType.CompanyAdmin.ToString()));
    options.AddPolicy("User", policy => policy.RequireRole(RoleType.User.ToString()));
});
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
builder.Services.AddScoped<ICurrentUser, CurrentUser>();
builder.Services.AddScoped<IEmail, Email>();
builder.Services.AddAutoMapper(AutoMapperProfiles.GetAssemblies());
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

