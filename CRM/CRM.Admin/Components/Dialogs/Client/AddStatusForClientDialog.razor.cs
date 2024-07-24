using CRM.Admin.Data.ClientDto;
using CRM.Admin.Data.ClientStatusHistoryDto;
using CRM.Admin.Requests.ClientRequests;
using CRM.Admin.Requests.ClientStatusHistoryRequests;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Dialogs.Client;

public partial class AddStatusForClientDialog
{
    [Inject] private IClientStatusHistoryRequest ClientStatusHistoryRequest { get; set; } = null!;
    [Inject] private IClientRequest ClientRequest { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public Guid Id { get; set; }

    private ClientStatusHistoryCreateDto ClientStatusHistoryCreateDto { get; set; } = new();
    private ClientUpdateDto ClientDto { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        ClientDto = await ClientRequest.GetByIdAsync(Id);
    }

    private async Task Update()
    {
        ClientStatusHistoryCreateDto.ClientId = Id;
        ClientStatusHistoryCreateDto.DateTime = DateTime.UtcNow;

        var result = await ClientStatusHistoryRequest.CreateAsync(ClientStatusHistoryCreateDto);
        if (result != Guid.Empty)
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