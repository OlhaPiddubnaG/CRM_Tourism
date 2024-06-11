using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace CRM.Admin.Components.Layout;

public partial class MainLayout
{
    [Inject] AuthenticationStateProvider AuthenticationStateProvider  {get; set; }
    
    private ClaimsPrincipal user;
    private bool isAdmin;
    
    bool open = false;
    bool dense = false;
    bool preserveOpenState = false;
    Breakpoint breakpoint = Breakpoint.Lg;
    DrawerClipMode clipMode = DrawerClipMode.Never;
  
    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        user = authState.User;

        if (user.Identity.IsAuthenticated)
        {
            isAdmin = user.IsInRole("Admin");
        }
    }
    
    void ToggleDrawer()
    {
        open = !open;
    }
}

