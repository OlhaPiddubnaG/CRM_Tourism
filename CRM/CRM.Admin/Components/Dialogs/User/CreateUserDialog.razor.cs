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

    private UserCreateDto _userCreateDto { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        _userCreateDto.CompanyId = Id;
        _userCreateDto.Password = Constants.DefaultPassword;
    }

    private async Task Create()
    {
        if (string.IsNullOrEmpty(_userCreateDto.Name) || string.IsNullOrEmpty(_userCreateDto.Surname) ||
            string.IsNullOrEmpty(_userCreateDto.Email))
        {
            Snackbar.Add("Будь ласка, заповніть всі поля", Severity.Error);
            return;
        }

        if (_userCreateDto.RoleTypes == null)
        {
            Snackbar.Add("Будь ласка, виберіть хоча б одну роль", Severity.Error);
            return;
        }

        var result = await SuperAdminRequest.CreateUserAsync(_userCreateDto);
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