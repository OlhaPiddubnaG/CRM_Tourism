using CRM.Admin.Data.CompanyDto;
using CRM.Admin.Requests.CompanyRequests;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Dialogs.Company;

public partial class UpdateCompanyDialog
{
    [Inject] private ICompanyRequest CompanyRequest { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public Guid Id { get; set; }

    private CompanyUpdateDto _сompanyUpdateDto { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        var result = _сompanyUpdateDto = await CompanyRequest.GetByIdAsync(Id);
        if (result.Id != Guid.Empty)
        {
            Snackbar.Add("Дані компанії завантажено", Severity.Success);
        }
        else
        {
            Snackbar.Add($"Помилка завантаження користувача", Severity.Error);
        }
    }

    private async Task Update()
    {
        var result = await CompanyRequest.UpdateAsync(_сompanyUpdateDto);
        if (result)
        {
            Snackbar.Add("Назву компанії змінено", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
        else
        {
            Snackbar.Add($"Помилка при внесенні змін в назву компанії", Severity.Error);
        }
    }

    void Cancel() => MudDialog.Cancel();
}