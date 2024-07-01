using CRM.Admin.Data.CompanyDto;
using CRM.Admin.Requests.SuperAdminRequests;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Dialogs.Company;

public partial class CreateCompanyDialog
{
    [Inject] private ISuperAdminRequest SuperAdminRequest { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    
    [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;
    
    private CompanyCreateDto CompanyCreateDto { get; set; } = new();

    private async Task Create()
    {
        if (string.IsNullOrEmpty(CompanyCreateDto.Name))
        {
            Snackbar.Add("Назва компанії не може бути порожньою", Severity.Error);
            return;
        }
        try
        {
            await SuperAdminRequest.CreateCompanyAsync(CompanyCreateDto);
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