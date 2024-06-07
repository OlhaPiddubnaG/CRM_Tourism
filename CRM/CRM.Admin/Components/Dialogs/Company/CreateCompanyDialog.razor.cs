using CRM.Admin.Data.CompanyDTO;
using CRM.Admin.Requests.SuperAdminRequests;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace CRM.Admin.Components.Dialogs.Company;

public partial class CreateCompanyDialog
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }
    [Inject] ISuperAdminRequest SuperAdminRequest { get; set; }
    [Inject] ISnackbar Snackbar { get; set; }
    private CompanyCreateDTO companyCreateDTO { get; set; } = new();
    
    private async Task SubmitExperience(EditContext editContext)
    {
        if (editContext.Validate())
        {
            await SuperAdminRequest.CreateCompany(companyCreateDTO);
            Snackbar.Add("Created", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));           
        }
    }

    void Cancel() => MudDialog.Cancel();
}