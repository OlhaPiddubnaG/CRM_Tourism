using CRM.Admin.Data.CompanyDTO;
using CRM.Admin.Requests.SuperAdminRequests;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Dialogs.Company;

public partial class CreateCompanyDialog
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }
    [Inject] ISuperAdminRequest SuperAdminRequest { get; set; }
    [Inject] ISnackbar Snackbar { get; set; }
    private CompanyCreateDTO companyCreateDTO { get; set; } = new();

    private async Task Create()
    {
        if (string.IsNullOrEmpty(companyCreateDTO.Name))
        {
            Snackbar.Add("Назва компанії не може бути порожньою", Severity.Error);
            return;
        }
        try
        {
            await SuperAdminRequest.CreateCompanyAsync(companyCreateDTO);
            Snackbar.Add("Створено", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Помилка: {ex.Message}", Severity.Error);
        }
    }

    void Cancel() => MudDialog.Cancel();
}