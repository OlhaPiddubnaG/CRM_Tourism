using System.Security.Claims;
using CRM.Admin.Data.UserDTO;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace CRM.Admin.Components.Pages;

public partial class ForgotPassword
{
    private UserDTO user;
    public string LoginMesssage { get; set; }
    ClaimsPrincipal claimsPrincipal;

    [CascadingParameter] private Task<AuthenticationState> authenticationStateTask { get; set; }

    protected async override Task OnInitializedAsync()
    {
        user = new UserDTO();

        claimsPrincipal = (await authenticationStateTask).User;

        if (claimsPrincipal.Identity.IsAuthenticated)
        {
            NavigationManager.NavigateTo("/index");
        }

        {
            // user.EmailAddress = "philip.cramer@gmail.com";
            // user.Password = "philip.cramer";
        }
    }

    private void ValidateUser()
    {
    }
}