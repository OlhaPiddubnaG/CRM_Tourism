using CRM.Admin.Data.TouroperatorDto;
using CRM.Admin.Requests.TouroperatorRequests;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Dialogs.Touroperator;

public partial class CreateTouroperatorDialog
{
    [Inject] private ITouroperatorRequest TouroperatorRequest { get; set; } = null!;
    [Inject] private ISnackbar? Snackbar { get; set; }

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public Guid Id { get; set; }

    private TouroperatorCreateDto _touroperatorCreateDto { get; set; } = new();

    private async Task Create()
    {
        _touroperatorCreateDto.CompanyId = Id;
        var touroperatorId = await TouroperatorRequest.CreateAsync(_touroperatorCreateDto);
        if (touroperatorId != Guid.Empty)
        {
            Snackbar.Add("Додано туроператора", Severity.Success);
            MudDialog.Close(DialogResult.Ok(new TouroperatorDto
                { Id = touroperatorId, Name = _touroperatorCreateDto.Name }));
        }
        else
        {
            Snackbar.Add($"Помилка при додаванні туроператора", Severity.Error);
        }
    }

    void Cancel() => MudDialog.Cancel();
}