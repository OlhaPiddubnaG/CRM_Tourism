using CRM.Admin.Data.ClientDTO;
using CRM.Admin.Data.UserDTO;
using CRM.Admin.Requests.ClientRequests;
using CRM.Admin.Requests.UserRequests;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Dialogs.Client;

public partial class AddManagerForClientDialog
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }
    [Inject] IClientRequest ClientRequest { get; set; }
    [Inject] IUserRequest UserRequest { get; set; }
    [Inject] ISnackbar Snackbar { get; set; }
    private ClientUpdateDTO clientUpdateDTO { get; set; } = new();
    [Parameter] public Guid Id { get; set; }
    private IEnumerable<Guid> selectedManagerIds = new List<Guid>();
    private IEnumerable<UserDTO> managers = new List<UserDTO>();

    protected override async Task OnInitializedAsync()
    {
        managers = (await UserRequest.GetAllAsync()).Where(u => !u.IsDeleted).ToList();
        clientUpdateDTO = await ClientRequest.GetByIdAsync<ClientUpdateDTO>(Id);
    }

    private async Task Create()
    {
        try
        {
            clientUpdateDTO.ManagerIds = selectedManagerIds;
            await ClientRequest.UpdateAsync(clientUpdateDTO);
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