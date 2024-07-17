using CRM.Admin.Components.Dialogs.Client;
using CRM.Admin.Data.ClientDto;
using CRM.Admin.Data.ClientPrivateDataDto;
using CRM.Admin.Data.PassportInfoDto;
using CRM.Admin.Requests.ClientPrivateDataRequests;
using CRM.Admin.Requests.ClientRequests;
using CRM.Admin.Requests.PassportInfoRequests;
using CRM.Domain.Enums;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Pages.Client;

public partial class ClientById
{
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IDialogService DialogService { get; set; } = null!;
    [Inject] private IClientRequest ClientRequest { get; set; } = null!;
    [Inject] private IClientPrivateDataRequest ClientPrivateDataRequest { get; set; } = null!;
    [Inject] private IPassportInfoRequest PassportInfoRequest { get; set; } = null!;
    
    [Parameter] public required string ClientId { get; set; } 
    
    private readonly DialogOptions _dialogOptions = new()
    {
        CloseOnEscapeKey = true,
        CloseButton = true,
        DisableBackdropClick = true,
        MaxWidth = MaxWidth.Small,
        FullWidth = true
    }; 
    private IEnumerable<PassportInfoDto> _passportInfoDtos = new List<PassportInfoDto>();
    private PassportInfoDto _passportInternalDto = new ();
    private PassportInfoDto _passportInternationalDto = new ();
    private ClientPrivateDataDto _clientPrivateDataDto = new ();
    private ClientDto _clientDto = new (); 
    private Guid _id;
   
    protected override async Task OnInitializedAsync()
    {
        _id = Guid.Parse(ClientId);
        _clientDto = await ClientRequest.GetByIdAsync<ClientDto>(_id);
        _clientPrivateDataDto = await ClientPrivateDataRequest.GetByClientIdAsync<ClientPrivateDataDto>(_id);

        _passportInfoDtos = await PassportInfoRequest.GetByClientPrivateDataIdAsync(_clientPrivateDataDto.Id);

        _passportInternalDto = _passportInfoDtos.FirstOrDefault(s => s.PassportType == PassportType.Internal) ?? new PassportInfoDto();
        _passportInternationalDto = _passportInfoDtos.FirstOrDefault(s => s.PassportType == PassportType.International) ?? new PassportInfoDto();
    }
    
    private async Task ChangeClientInfo()
    {
        var parameters = new DialogParameters { { "Id", _id } };
        var dialogReference = await DialogService.ShowAsync<UpdateClientDialog>("",parameters, _dialogOptions);
        var dialogResult = await dialogReference.Result;

        if (dialogResult.Canceled)
            return;
    } 
    
    private async Task ChangePassportInfo()
    {
        NavigationManager.NavigateTo($"/updatePassportInfo/{ClientId}");
    }
}