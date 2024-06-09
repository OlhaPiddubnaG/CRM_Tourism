using CRM.Admin.Data.UserDTO;
using CRM.Admin.Requests.SuperAdminRequests;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
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
        userCreateDTO.Password = "123456789Qaz";
    }

    private async Task SubmitExperience(EditContext editContext)
    {
        if (editContext.Validate())
        {
            await SuperAdminRequest.CreateCompanyAdmin(userCreateDTO);
            Snackbar.Add("Створено", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));           
        }
    }

    void Cancel() => MudDialog.Cancel();
}
