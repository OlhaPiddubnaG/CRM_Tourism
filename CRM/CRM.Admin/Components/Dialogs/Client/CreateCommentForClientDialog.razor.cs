using CRM.Admin.Data.ClientDto;
using CRM.Admin.Requests.ClientRequests;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Dialogs.Client;

public partial class CreateCommentForClientDialog
{
    [Inject] private IClientRequest ClientRequest { get; set; } = null!;
    [Inject] private ISnackbar? Snackbar { get; set; }

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public Guid Id { get; set; }

    private ClientUpdateDto _сlientUpdateDto { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        _сlientUpdateDto = await ClientRequest.GetByIdAsync<ClientUpdateDto>(Id);
    }

    private async Task Create()
    {
        var result = await ClientRequest.UpdateAsync(_сlientUpdateDto);
        if (result)
        {
            Snackbar.Add("Додано коментар", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
        else
        {
            Snackbar.Add($"Помилка при додаванні коментаря", Severity.Error);
        }
    }

    void Cancel() => MudDialog.Cancel();
}