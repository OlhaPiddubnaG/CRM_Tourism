using CRM.Admin.Components;
using CRM.Admin.HttpRequests;
using CRM.Admin.Requests.AuthRequests;
using Microsoft.AspNetCore.Components;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddMudServices();

var baseUri = new Uri(builder.Configuration["BaseAddresses:ApiBaseUrl"] ??
                      throw new Exception("Cannot find 'BaseAddresses:ApiBaseUrl'"));

builder.Services
    .AddHttpClient("CRMApi", client => client.BaseAddress = baseUri);

builder.Services.AddScoped<IHttpCRMApiRequests, HttpRequests>(
    serviceProvider => new HttpRequests(
        factory: serviceProvider.GetRequiredService<IHttpClientFactory>(),
        navManager: serviceProvider.GetRequiredService<NavigationManager>(),
        clientName: "CRMApi")
);
builder.Services.AddTransient<IAuthenticationRequest, AuthenticationRequest>();

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();