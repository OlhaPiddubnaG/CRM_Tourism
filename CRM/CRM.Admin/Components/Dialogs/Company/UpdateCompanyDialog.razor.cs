using CRM.Admin.Data.CompanyDTO;
using CRM.Admin.Requests.CompanyRequests;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace CRM.Admin.Components.Dialogs.Company;

public partial class UpdateCompanyDialog
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }
    [Parameter] public Guid Id { get; set; }
    [Inject] ICompanyRequest CompanyRequest { get; set; }
    [Inject] ISnackbar Snackbar { get; set; }
    private CompanyUpdateDTO companyUpdateDTO { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        companyUpdateDTO = await CompanyRequest.GetByIdAsync<CompanyUpdateDTO>(Id);
    }

    private async Task SubmitExperience(EditContext editContext)
    {
        if (editContext.Validate())
        {
            await CompanyRequest.UpdateAsync(companyUpdateDTO);
            Snackbar.Add("Updated", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
    }

    void Cancel() => MudDialog.Cancel();
}
