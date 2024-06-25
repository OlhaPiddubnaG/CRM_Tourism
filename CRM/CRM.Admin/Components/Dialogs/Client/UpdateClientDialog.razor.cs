using CRM.Admin.Data.ClientDTO;
using CRM.Admin.Requests.ClientRequests;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Dialogs.Client;

public partial class UpdateClientDialog
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }
    [Inject] ISnackbar Snackbar { get; set; }
    [Inject] IClientRequest ClientRequest { get; set; }
    [Parameter] public Guid Id { get; set; }
    
    private ClientUpdateDTO clientUpdateDTO { get; set; } = new();
    private DateTime? dateOfBirth;
    
    protected override async Task OnInitializedAsync()
    {
        try
        {
            clientUpdateDTO = await ClientRequest.GetByIdAsync<ClientUpdateDTO>(Id);
            dateOfBirth = clientUpdateDTO.DateOfBirth?.ToDateTime(TimeOnly.MinValue);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Помилка завантаження користувача: {ex.Message}", Severity.Error);
        }
       
    }

    private async Task UpdateClientAsync()
    {
        try
        {
            clientUpdateDTO.DateOfBirth = dateOfBirth.HasValue ? DateOnly.FromDateTime(dateOfBirth.Value) : clientUpdateDTO.DateOfBirth;
            await ClientRequest.UpdateAsync(clientUpdateDTO);
            Snackbar.Add("Внесено зміни", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Помилка: {ex.Message}", Severity.Error);
        }
    }

    private void Cancel() => MudDialog.Cancel();
}