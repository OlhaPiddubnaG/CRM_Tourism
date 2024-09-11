using CRM.Admin.Data.ClientDto;
using CRM.Admin.Requests.ClientRequests;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Dialogs.Client;

public partial class AddStatusForClientDialog
{
    [Inject] private IClientRequest ClientRequest { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public Guid Id { get; set; }

    private ClientUpdateDto _clientUpdateDto { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        _clientUpdateDto = await ClientRequest.GetByIdAsync(Id);
    }

    private async Task Update()
    {
        var result = await ClientRequest.UpdateAsync(_clientUpdateDto);
        if (result)
        {
            Snackbar.Add("Статус туриста змінено", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
        else
        {
            Snackbar.Add("Помилка при зміні статусу туриста", Severity.Error);
        }
    }

    void Cancel() => MudDialog.Cancel();
}