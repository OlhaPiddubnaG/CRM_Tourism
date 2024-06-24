using CRM.Admin.Data.ClientStatusHistoryDTO;
using CRM.Admin.Requests.ClientStatusHistoryRequests;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Dialogs.Client;

public partial class AddStatusForClientDialog
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }
    [Inject] IClientStatusHistoryRequest ClientStatusHistoryRequest { get; set; }
    [Inject] ISnackbar Snackbar { get; set; }
    private ClientStatusHistoryCreateDTO clientStatusHistoryCreateDTO { get; set; } = new();
    [Parameter] public Guid Id { get; set; }
    
    private async Task Create()
    {
        try
        {
            clientStatusHistoryCreateDTO.ClientId = Id;
            clientStatusHistoryCreateDTO.DateTime = DateTime.UtcNow;
            
            await ClientStatusHistoryRequest.CreateAsync(clientStatusHistoryCreateDTO);
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