using CRM.Admin.Data.UserDTO;
using CRM.Admin.Requests.UserRequests;
using CRM.Domain.Enums;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Dialogs.User;

public partial class UpdateUserDialog
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }
    [Parameter] public Guid Id { get; set; }
    [Inject] IUserRequest UserRequest { get; set; }
    [Inject] ISnackbar Snackbar { get; set; }
    private UserUpdateDTO userUpdateDTO { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        try
        {
            userUpdateDTO = await UserRequest.GetByIdAsync<UserUpdateDTO>(Id);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Помилка завантаження користувача: {ex.Message}", Severity.Error);
        }
    }

    private async Task Update()
    {
        try
        {
            userUpdateDTO.RoleTypes ??= new List<RoleType>();
            await UserRequest.UpdateAsync(userUpdateDTO);
            Snackbar.Add("Внесено зміни", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Помилка: {ex.Message}", Severity.Error);
        }
    }

    void Cancel() => MudDialog.Cancel();
}