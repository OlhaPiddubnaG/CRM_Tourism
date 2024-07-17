using CRM.Admin.Data.UserDto;
using CRM.Admin.Requests.UserRequests;
using CRM.Domain.Enums;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Dialogs.User;

public partial class UpdateUserDialog
{
    [Inject] private IUserRequest UserRequest { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public Guid Id { get; set; }

    private UserUpdateDto _userUpdateDto { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        var result = _userUpdateDto = await UserRequest.GetByIdAsync<UserUpdateDto>(Id);
        if (result.Id != Guid.Empty)
        {
            Snackbar.Add("Дані користувача завантажено", Severity.Success);
        }
        else
        {
            Snackbar.Add($"Помилка завантаження користувача", Severity.Error);
        }
    }

    private async Task Update()
    {
        _userUpdateDto.RoleTypes ??= new List<RoleType>();
        var result = await UserRequest.UpdateAsync(_userUpdateDto);
        if (result)
        {
            Snackbar.Add("Внесено зміни до даних користувача", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
        else
        {
            Snackbar.Add($"Помилка при внесенні змінних до даних користувача", Severity.Error);
        }
    }

    void Cancel() => MudDialog.Cancel();
}