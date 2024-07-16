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

    private UserCreateDto _userCreateDTO { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        _userCreateDTO.CompanyId = Id;
        _userCreateDTO.Password = "123456789Qaz";
    }

    private async Task Create()
    {
        if (string.IsNullOrEmpty(_userCreateDTO.Name) || string.IsNullOrEmpty(_userCreateDTO.Surname) ||
            string.IsNullOrEmpty(_userCreateDTO.Email))
        {
            Snackbar.Add("Будь ласка, заповніть всі поля", Severity.Error);
            return;
        }

        if (_userCreateDTO.RoleTypes == null)
        {
            Snackbar.Add("Будь ласка, виберіть хоча б одну роль", Severity.Error);
            return;
        }

        var result = await UserRequest.CreateAsync(_userCreateDTO);
        if (result != Guid.Empty)
        {
            Snackbar.Add("Створено користувача", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
        else
        {
            Snackbar.Add($"Помилка при створенні користувача", Severity.Error);
        }
    }

    void Cancel() => MudDialog.Cancel();
}