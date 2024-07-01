using CRM.Admin.Data.UserDto;
using CRM.Admin.Requests.UserRequests;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Dialogs.User;

public partial class CreateStaffDialog
{
    [Inject] IUserRequest UserRequest { get; set; }
    [Inject] ISnackbar Snackbar { get; set; }
    
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }
    [Parameter] public Guid Id { get; set; }
    
    private UserCreateDto userCreateDTO { get; set; } = new();
    
    protected override async Task OnInitializedAsync()
    {
        userCreateDTO.CompanyId = Id;
        userCreateDTO.Password = "123456789Qaz";
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
            await UserRequest.CreateAsync(userCreateDTO);
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
