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

    private ClientUpdateDto ClientUpdateDto { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        ClientUpdateDto = await ClientRequest.GetByIdAsync<ClientUpdateDto>(Id);
    }

    private async Task Create()
    {
        try
        {
            await ClientRequest.UpdateAsync(ClientUpdateDto);
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