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
    
    private UserUpdateDto UserUpdateDto { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        try
        {
            UserUpdateDto = await UserRequest.GetByIdAsync<UserUpdateDto>(Id);
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
            UserUpdateDto.RoleTypes ??= new List<RoleType>();
            await UserRequest.UpdateAsync(UserUpdateDto);
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