using CRM.Admin.Data.UserDTO;
using CRM.Admin.Requests.SuperAdminRequests;
using CRM.Domain.Constants;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Dialogs.User;

public partial class CreateUserDialog
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }
    [Inject] ISuperAdminRequest SuperAdminRequest { get; set; }
    [Inject] ISnackbar Snackbar { get; set; }
    private UserCreateDTO userCreateDTO { get; set; } = new();
    [Parameter] public Guid Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        userCreateDTO.CompanyId = Id;
        userCreateDTO.Password = Constants.DefaultPassword;
    }

    private async Task Create()
    {
        if (string.IsNullOrEmpty(userCreateDTO.Name) || string.IsNullOrEmpty(userCreateDTO.Surname) ||
            string.IsNullOrEmpty(userCreateDTO.Email))
        {
            Snackbar.Add("Будь ласка, заповніть всі поля", Severity.Error);
            return;
        }

        if (userCreateDTO.RoleTypes == null)
        {
            Snackbar.Add("Будь ласка, виберіть хоча б одну роль", Severity.Error);
            return;
        }

        try
        {
            await SuperAdminRequest.CreateUserAsync(userCreateDTO);
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