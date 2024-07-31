using CRM.Admin.Auth;
using CRM.Admin.Components;
using CRM.Admin.Extensions;
using CRM.Admin.HttpRequests;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
     .AddInteractiveServerComponents();
builder.Services.AddMudServices();

var baseUri = new Uri(builder.Configuration["BaseAddresses:ApiBaseUrl"] ??
                      throw new Exception("Cannot find 'BaseAddresses:ApiBaseUrl'"));

builder.Services
    .AddHttpClient("CRMApi", client => client.BaseAddress = baseUri);

builder.Services.AddScoped<IHttpRequests, HttpRequests>(
    serviceProvider => new HttpRequests(
        factory: serviceProvider.GetRequiredService<IHttpClientFactory>(),
        navManager: serviceProvider.GetRequiredService<NavigationManager>(),
        jsRuntime: serviceProvider.GetRequiredService<IJSRuntime>(),
        clientName: "CRMApi")
);
builder.Services.AddCustomServices();

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuthenticationStateProvider>());

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthState>();
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