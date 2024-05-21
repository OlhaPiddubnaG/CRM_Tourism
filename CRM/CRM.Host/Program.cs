using CRM.DataAccess;
using CRM.DataAccess.InitialAdminSettings;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("AppDbContext"),
        optionsBuilder => optionsBuilder.MigrationsAssembly("CRM.DataAccess")));

builder.Services.Configure<InitialAdminSettings>(builder.Configuration.GetSection(InitialAdminSettings.Section));
builder.Services.AddHostedService<MigrationsService>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.Run();

