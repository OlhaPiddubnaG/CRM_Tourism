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

    private ClientUpdateDto _сlientUpdateDto { get; set; } = new();
    private DateTime? _dateOfBirth;

    protected override async Task OnInitializedAsync()
    {
        _сlientUpdateDto = await ClientRequest.GetByIdAsync<ClientUpdateDto>(Id);
        _dateOfBirth = _сlientUpdateDto.DateOfBirth?.ToDateTime(TimeOnly.MinValue);
    }

    private async Task UpdateClientAsync()
    {
        _сlientUpdateDto.DateOfBirth = _dateOfBirth.HasValue
            ? DateOnly.FromDateTime(_dateOfBirth.Value)
            : _сlientUpdateDto.DateOfBirth;
        _сlientUpdateDto.DateOfBirth = _dateOfBirth.HasValue
            ? DateOnly.FromDateTime(_dateOfBirth.Value)
            : _сlientUpdateDto.DateOfBirth;
        var result = await ClientRequest.UpdateAsync(_сlientUpdateDto);

        if (result)
        {
            Snackbar.Add("Дані клієнта успішно оновлені");
            MudDialog.Close(DialogResult.Ok(true));
        }
        else
        {
            Snackbar.Add("Помилка оновлення клієнта");
        }
    }

    private void Cancel() => MudDialog.Cancel();
}