using CRM.Admin.Data.UserDto;
using CRM.Admin.Requests.SuperAdminRequests;
using CRM.Domain.Constants;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Dialogs.User;

public partial class CreateUserDialog
{
    [Inject] private ISuperAdminRequest SuperAdminRequest { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    
    [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public Guid Id { get; set; }
    
    private UserCreateDto UserCreateDto { get; set; } = new();
    
    protected override async Task OnInitializedAsync()
    {
        UserCreateDto.CompanyId = Id;
        UserCreateDto.Password = Constants.DefaultPassword;
    }

    private async Task Create()
    {
        if (string.IsNullOrEmpty(UserCreateDto.Name) || string.IsNullOrEmpty(UserCreateDto.Surname) ||
            string.IsNullOrEmpty(UserCreateDto.Email))
        {
            Snackbar.Add("Будь ласка, заповніть всі поля", Severity.Error);
            return;
        }

        if (UserCreateDto.RoleTypes == null)
        {
            Snackbar.Add("Будь ласка, виберіть хоча б одну роль", Severity.Error);
            return;
        }

        try
        {
            await SuperAdminRequest.CreateUserAsync(UserCreateDto);
            Snackbar.Add("Створено", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Помилка: {ex.Message}", Severity.Error);
        }
    }

    void Cancel() => MudDialog.Cancel();
}