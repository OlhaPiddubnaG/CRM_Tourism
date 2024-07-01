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
    
    private CompanyUpdateDto CompanyUpdateDto { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        CompanyUpdateDto = await CompanyRequest.GetByIdAsync<CompanyUpdateDto>(Id);
    }

    private async Task Update()
    {
        try
        {
            await CompanyRequest.UpdateAsync(CompanyUpdateDto);
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