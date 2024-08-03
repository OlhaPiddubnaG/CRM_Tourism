using CRM.Admin.Data.CountryDto;
using CRM.Admin.Requests.CountryRequests;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Dialogs.Country;

public partial class CreateCountryDialog
{
    [Inject] private ICountryRequest CountryRequest { get; set; } = null!;
    [Inject] private ISnackbar? Snackbar { get; set; }

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public Guid Id { get; set; }

    private CountryCreateDto _countryCreateDto { get; set; } = new();

    private async Task Create()
    {
        _countryCreateDto.CompanyId = Id;
        var countryId = await CountryRequest.CreateAsync(_countryCreateDto);
        if (countryId != Guid.Empty)
        {
            Snackbar.Add("Додано країну", Severity.Success);
            MudDialog.Close(DialogResult.Ok(new CountryDto { Id = countryId, Name = _countryCreateDto.Name }));
        }
        else
        {
            Snackbar.Add($"Помилка при додаванні країни", Severity.Error);
        }
    }

    void Cancel() => MudDialog.Cancel();
}