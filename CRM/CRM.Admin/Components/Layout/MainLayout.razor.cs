using System.Security.Claims;
using CRM.Admin.Auth;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Layout;

public partial class MainLayout
{
    [Inject] private CustomAuthenticationStateProvider AuthenticationStateProvider  {get; set; }
    
    private ClaimsPrincipal _user;
    private bool _isAdmin;
    
    bool _open = false;
    bool _dense = false;
    bool _preserveOpenState = false;
    Breakpoint _breakpoint = Breakpoint.Lg;
    DrawerClipMode _clipMode = DrawerClipMode.Never;
  
    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        _user = authState.User;

        if (_user.Identity.IsAuthenticated)
        {
            _isAdmin = _user.IsInRole("Admin");
        }
    }
    
    void ToggleDrawer()
    {
        _open = !_open;
    }
}

