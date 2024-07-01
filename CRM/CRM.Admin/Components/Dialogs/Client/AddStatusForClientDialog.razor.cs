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
    private ClientDto ClientDto { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        ClientDto = await ClientRequest.GetByIdAsync<ClientDto>(Id);
    }

    private async Task Update()
    {
        try
        {
            ClientStatusHistoryCreateDto.ClientId = Id;
            ClientStatusHistoryCreateDto.DateTime = DateTime.UtcNow;

            await ClientStatusHistoryRequest.CreateAsync(ClientStatusHistoryCreateDto);
            Snackbar.Add("Статус туриста змінено", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Помилка: {ex.Message}", Severity.Error);
        }
    }

    void Cancel() => MudDialog.Cancel();
}