using CRM.Admin.Data.UserDTO;
using CRM.Admin.Requests.UserRequests;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
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
        userUpdateDTO = await UserRequest.GetByIdAsync<UserUpdateDTO>(Id);
    }

    private async Task SubmitExperience(EditContext editContext)
    {
        if (editContext.Validate())
        {
            await UserRequest.UpdateAsync(userUpdateDTO);
            Snackbar.Add("Внесено зміни", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
    }

    void Cancel() => MudDialog.Cancel();
}
