using CRM.Admin.Data.CompanyDTO;
using CRM.Admin.Requests.CompanyRequests;
using Microsoft.AspNetCore.Components;
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

    private async Task Update()
    {
        try
        {
            await CompanyRequest.UpdateAsync(companyUpdateDTO);
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