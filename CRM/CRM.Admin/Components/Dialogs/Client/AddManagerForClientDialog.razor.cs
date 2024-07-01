using CRM.Admin.Data.ClientDto;
using CRM.Admin.Data.UserDto;
using CRM.Admin.Requests.ClientRequests;
using CRM.Admin.Requests.UserRequests;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Dialogs.Client;

public partial class AddManagerForClientDialog
{
    [Inject] private IClientRequest ClientRequest { get; set; } = null!;
    [Inject] private IUserRequest UserRequest { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public Guid Id { get; set; }

    private IEnumerable<Guid> _selectedManagerIds = new List<Guid>();
    private IEnumerable<UserDto> _managers = new List<UserDto>();
    private ClientUpdateDto ClientUpdateDto { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        _managers = await UserRequest.GetAllAsync();
        ClientUpdateDto = await ClientRequest.GetByIdAsync<ClientUpdateDto>(Id);
    }

    private async Task Create()
    {
        try
        {
            ClientUpdateDto.ManagerIds = _selectedManagerIds;
            await ClientRequest.UpdateAsync(ClientUpdateDto);
            Snackbar.Add("Додано менеджера", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Помилка: {ex.Message}", Severity.Error);
        }
    }

    void Cancel() => MudDialog.Cancel();
}