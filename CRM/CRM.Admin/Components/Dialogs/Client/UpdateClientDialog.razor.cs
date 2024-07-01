using CRM.Admin.Data.ClientDto;
using CRM.Admin.Requests.ClientRequests;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Dialogs.Client;

public partial class UpdateClientDialog
{
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IClientRequest ClientRequest { get; set; } = null!;

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public Guid Id { get; set; }

    private ClientUpdateDto ClientUpdateDto { get; set; } = new();
    private DateTime? _dateOfBirth;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            ClientUpdateDto = await ClientRequest.GetByIdAsync<ClientUpdateDto>(Id);
            _dateOfBirth = ClientUpdateDto.DateOfBirth?.ToDateTime(TimeOnly.MinValue);
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
            ClientUpdateDto.DateOfBirth = _dateOfBirth.HasValue
                ? DateOnly.FromDateTime(_dateOfBirth.Value)
                : ClientUpdateDto.DateOfBirth;
            await ClientRequest.UpdateAsync(ClientUpdateDto);
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