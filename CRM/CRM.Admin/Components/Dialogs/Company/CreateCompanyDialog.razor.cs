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

    private CompanyCreateDto _сompanyCreateDto { get; set; } = new();

    private async Task Create()
    {
        if (string.IsNullOrEmpty(_сompanyCreateDto.Name))
        {
            Snackbar.Add("Назва компанії не може бути порожньою", Severity.Error);
            return;
        }

        var result = await SuperAdminRequest.CreateCompanyAsync(_сompanyCreateDto);
        if (result != Guid.Empty)
        {
            Snackbar.Add("Компанію створено", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
        else
        {
            Snackbar.Add($"Помилка при створенні компанії", Severity.Error);
        }
    }

    void Cancel() => MudDialog.Cancel();
}