using System.Security.Claims;
using CRM.Admin.Components.Dialogs.Client;
using CRM.Admin.Components.Dialogs.Order;
using CRM.Helper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace CRM.Admin.Components.Layout;

public partial class AppBar
{
    [Inject] IDialogService DialogService { get; set; } = default!;
    [Inject] AuthenticationStateProvider AuthenticationStateProvider  { get; set; }
    
    
    private DialogOptions dialogOptions = new()
    {
        CloseOnEscapeKey = true,
        CloseButton = true,
        DisableBackdropClick = true,
        MaxWidth = MaxWidth.Small,
        FullWidth = true
    };
    private ClaimsPrincipal user;
    private Guid companyId;
    bool disabled = false;
    
    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        user = authState.User;
        companyId = GetCompanyId();
    }

    private Guid GetCompanyId()
    {
        var companyIdClaim = user.FindFirst(CustomClaimTypes.CompanyId)?.Value;
        if (Guid.TryParse(companyIdClaim, out Guid companyId))
        {
            return companyId;
        }
        
        throw new InvalidOperationException("Invalid CompanyId claim.");
    }

    private async Task NewClient()
    {
        var parameters = new DialogParameters { { "Id", companyId } };
        var dialogReference = await DialogService.ShowAsync<CreateClientDialog>("",parameters, dialogOptions);
        var dialogResult = await dialogReference.Result;

        if (dialogResult.Canceled)
            return;
    }

    private async Task NewOrder()
    {
        var dialogReference = await DialogService.ShowAsync<CreateOrderDialog>("", dialogOptions);
        var dialogResult = await dialogReference.Result;

        if (dialogResult.Canceled)
            return;
    }
}