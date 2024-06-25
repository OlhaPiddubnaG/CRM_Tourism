using CRM.Admin.Components.Dialogs.Client;
using CRM.Admin.Data.ClientDTO;
using CRM.Admin.Data.ClientPrivateDataDTO;
using CRM.Admin.Data.PassportInfoDTO;
using CRM.Admin.Requests.ClientPrivateDataRequests;
using CRM.Admin.Requests.ClientRequests;
using CRM.Admin.Requests.PassportInfoRequests;
using CRM.Domain.Enums;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Pages.Client;

public partial class ClientById
{
    [Inject] NavigationManager NavigationManager { get; set; }
    [Inject] IDialogService DialogService { get; set; } = default!;
    [Inject] IClientRequest ClientRequest { get; set; } = default!;
    [Inject] IClientPrivateDataRequest ClientPrivateDataRequest { get; set; } = default!;
    [Inject] IPassportInfoRequest PassportInfoRequest { get; set; } = default!;
    
    private DialogOptions dialogOptions = new()
    {
        CloseOnEscapeKey = true,
        CloseButton = true,
        DisableBackdropClick = true,
        MaxWidth = MaxWidth.Small,
        FullWidth = true
    };
    [Parameter] public string clientId { get; set; } = default!;
    private ClientDTO clientDTO = new (); 
    private PassportInfoDTO passportInternalDTO = new ();
    private PassportInfoDTO passportInternationalDTO = new ();
    private IEnumerable<ClientPrivateDataDTO> clientPrivateDataDTOs = new List<ClientPrivateDataDTO>();
    private IEnumerable<PassportInfoDTO> passportInfoDTOs = new List<PassportInfoDTO>();
   
    protected override async Task OnInitializedAsync()
    {
        clientDTO = await ClientRequest.GetByIdAsync<ClientDTO>(Guid.Parse(clientId));
        clientPrivateDataDTOs = (await ClientPrivateDataRequest.GetAllAsync()).Where(i => i.ClientId == Guid.Parse(clientId));

        var clientPrivateDataIds = clientPrivateDataDTOs.Select(cpd => cpd.Id).ToList();
        passportInfoDTOs = (await PassportInfoRequest.GetAllAsync()).Where(pi => clientPrivateDataIds.Contains(pi.ClientPrivateDataId));

        passportInternalDTO = passportInfoDTOs.FirstOrDefault(s => s.PassportType == PassportType.Internal) ?? new PassportInfoDTO();
        passportInternationalDTO = passportInfoDTOs.FirstOrDefault(s => s.PassportType == PassportType.International) ?? new PassportInfoDTO();
    }
    
    private async Task ChangeClientInfo()
    {
        Guid id = Guid.Parse(clientId);
        var parameters = new DialogParameters { { "Id", id } };
        var dialogReference = await DialogService.ShowAsync<UpdateClientDialog>("",parameters, dialogOptions);
        var dialogResult = await dialogReference.Result;

        if (dialogResult.Canceled)
            return;
    } 
    private async Task ChangePassportInfo()
    {
        NavigationManager.NavigateTo($"/updatePassportInfo/{clientId}");
    }
}