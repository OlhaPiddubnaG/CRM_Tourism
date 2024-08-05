using System.Security.Claims;
using CRM.Admin.Auth;
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
    [Inject] private AuthenticationStateProvider AuthenticationStateProvider  { get; set; } = null!;
    [Inject] private AuthState AuthState  { get; set; } = null!;
    
    private DialogOptions _dialogOptions = new()
    {
        CloseOnEscapeKey = true,
        CloseButton = true,
        DisableBackdropClick = true,
        FullWidth = true
    };
    private ClaimsPrincipal _user;
    private Guid _companyId;
    bool _disabled = false;
    private Guid _userId;
    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        _user = authState.User;
        
        _companyId = GetCompanyId();
        AuthState.SetCompanyId(_companyId);
        
        _userId = GetUserId();
        AuthState.SetUserId(_userId);
    }

    private Guid GetCompanyId()
    {
        var companyIdClaim = _user.FindFirst(CustomClaimTypes.CompanyId)?.Value;
        if (Guid.TryParse(companyIdClaim, out Guid companyId))
        {
            return companyId;
        }
        
        throw new InvalidOperationException("Invalid CompanyId claim.");
    }
    
    private Guid GetUserId()
    {
        var userIdClaim = _user.FindFirst(CustomClaimTypes.UserId)?.Value;
        if (Guid.TryParse(userIdClaim, out Guid userId))
        {
            return userId;
        }
        
        throw new InvalidOperationException("Invalid CompanyId claim.");
    }

    /*private async Task NewClient()
    {
        var parameters = new DialogParameters { { "Id", _companyId } };
        _dialogOptions.MaxWidth = MaxWidth.Small;
         var dialogReference = await DialogService.ShowAsync<CreateClientWithRelatedDialog>("",parameters, _dialogOptions);
         var dialogResult = await dialogReference.Result;

        if (dialogResult.Canceled)
            return;
    }*/

    private async Task NewOrder()
    {
        var parameters = new DialogParameters { { "Id", _companyId } };
        _dialogOptions.MaxWidth = MaxWidth.ExtraSmall;
        var dialogReference = await DialogService.ShowAsync<CreateOrderDialog>("", parameters, _dialogOptions);
        var dialogResult = await dialogReference.Result;

        if (dialogResult.Canceled)
            return;
    }
}