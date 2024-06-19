using CRM.Admin.Data.ClientDTO;
using CRM.Admin.Requests.ClientRequests;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Dialogs.Client;

public partial class CreateCommentForClientDialog
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }
    [Inject] IClientRequest ClientRequest { get; set; }
    [Inject] ISnackbar Snackbar { get; set; }
    private ClientUpdateDTO clientUpdateDTO { get; set; } = new();
    [Parameter] public Guid Id { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        clientUpdateDTO = await ClientRequest.GetByIdAsync<ClientUpdateDTO>(Id);
    }
    private async Task Create()
    {
      
        try
        {
            await ClientRequest.UpdateAsync(clientUpdateDTO);
            Snackbar.Add("Додано", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Помилка: {ex.Message}", Severity.Error);
        }
    }

    void Cancel() => MudDialog.Cancel();
}
