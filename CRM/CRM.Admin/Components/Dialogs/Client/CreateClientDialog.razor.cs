using CRM.Admin.Data.ClientDTO;
using CRM.Admin.Data.ClientPrivateDataDTO;
using CRM.Admin.Data.ClientStatusHistoryDTO;
using CRM.Admin.Data.CountryDTO;
using CRM.Admin.Data.PassportInfoDTO;
using CRM.Admin.Data.UserDTO;
using CRM.Admin.Requests.ClientPrivateDataRequests;
using CRM.Admin.Requests.ClientRequests;
using CRM.Admin.Requests.ClientStatusHistoryRequests;
using CRM.Admin.Requests.CountryRequests;
using CRM.Admin.Requests.PassportInfoRequests;
using CRM.Admin.Requests.UserRequests;
using CRM.Domain.Enums;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Dialogs.Client;

public partial class CreateClientDialog
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }
    [Inject] NavigationManager NavigationManager { get; set; }
    [Inject] IClientRequest ClientRequest { get; set; }
    [Inject] IClientPrivateDataRequest ClientPrivateDataRequest { get; set; }
    [Inject] IClientStatusHistoryRequest ClientStatusHistoryRequest { get; set; }
    [Inject] IPassportInfoRequest PassportInfoRequest { get; set; }
    [Inject] ICountryRequest CountryRequest { get; set; }
    [Inject] IUserRequest UserRequest { get; set; }
    [Inject] ISnackbar Snackbar { get; set; }
    [Parameter] public Guid Id { get; set; }

    private ClientCreateDTO clientCreateDTO { get; set; } = new();
    private CountryCreateDTO countryCreateDTO { get; set; } = new();
    
    private ClientPrivateDataCreateDTO clientPrivateDataCreateDTO { get; set; } = new();
    private ClientStatusHistoryCreateDTO clientStatusHistoryCreateDTO { get; set; } = new();

    private PassportInfoCreateDTO passportInternalCreateDTO { get; set; } =
        new() { PassportType = PassportType.Internal };

    private PassportInfoCreateDTO passportInternationalCreateDTO { get; set; } =
        new() { PassportType = PassportType.International };

    private DateTime? dateOfBirth = DateTime.UtcNow;
    private DateTime? passportInternalExpiryDate = DateTime.UtcNow;
    private DateTime? passportInternalIssueDate = DateTime.UtcNow;
    private DateTime? passportInternationalExpiryDate = DateTime.UtcNow;
    private DateTime? passportInternationalIssueDate = DateTime.UtcNow;
    private int activeTabIndex = 0;
    private List<UserDTO> users = new (); 

    protected override async Task OnInitializedAsync()
    {
        users = (await UserRequest.GetAllAsync()).Where(u => !u.IsDeleted).ToList();
    }

    private async Task CreateClientAsync()
    {
        if (!IsClientDataValid())
        {
            Snackbar.Add("Будь ласка, заповніть всі поля", Severity.Error);
            return;
        }

        try
        {
            SetClientData();
            Guid countryId = await GetOrCreateCountryAsync();

            clientCreateDTO.CountryId = countryId;
            Guid clientId = await ClientRequest.CreateAsync(clientCreateDTO);
            if (clientId == Guid.Empty)
            {
                Snackbar.Add("Клієнт не створений", Severity.Error);
                return;
            }

            Snackbar.Add("Клієнт створений", Severity.Success);

            Guid clientPrivateDataId = await CreateClientPrivateDataAsync(clientId);
            Guid clientStatusHistoryId = await CreateClientStatusHistoryAsync(clientId);
            
            passportInternalCreateDTO.ClientPrivateDataId = clientPrivateDataId;
            passportInternalCreateDTO.ExpiryDate = passportInternalExpiryDate.HasValue
                ? DateOnly.FromDateTime(passportInternalExpiryDate.Value)
                : (DateOnly?)null;
            passportInternalCreateDTO.DateOfIssue = passportInternalIssueDate.HasValue
                ? DateOnly.FromDateTime(passportInternalIssueDate.Value)
                : (DateOnly?)null;

            await PassportInfoRequest.CreateAsync(passportInternalCreateDTO);
            Snackbar.Add("Інформація про внутрішній паспорт створена", Severity.Success);

            passportInternationalCreateDTO.ClientPrivateDataId = clientPrivateDataId;
            passportInternationalCreateDTO.ExpiryDate = passportInternationalExpiryDate.HasValue
                ? DateOnly.FromDateTime(passportInternationalExpiryDate.Value)
                : (DateOnly?)null;
            passportInternationalCreateDTO.DateOfIssue = passportInternationalIssueDate.HasValue
                ? DateOnly.FromDateTime(passportInternationalIssueDate.Value)
                : (DateOnly?)null;

            await PassportInfoRequest.CreateAsync(passportInternationalCreateDTO);
            Snackbar.Add("Інформація про закордонний паспорт створена", Severity.Success);

            NavigationManager.NavigateTo("/client");
            MudDialog.Close(DialogResult.Ok(true));
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Помилка: {ex.Message}", Severity.Error);
        }
    }

    private async Task<Guid> GetOrCreateCountryAsync()
    {
        var existingCountry = await CountryRequest.GetByNameAsync(countryCreateDTO.Name);
        if (existingCountry != null)
        {
            return existingCountry.Id;
        }

        return await CountryRequest.CreateAsync(countryCreateDTO);
    }

    private bool IsClientDataValid() =>
        !string.IsNullOrEmpty(clientCreateDTO.Name) && !string.IsNullOrEmpty(clientCreateDTO.Surname);

    private void SetClientData()
    {
        clientCreateDTO.CompanyId = Id;
        clientCreateDTO.DateOfBirth = dateOfBirth.HasValue ? DateOnly.FromDateTime(dateOfBirth.Value) : (DateOnly?)null;

        countryCreateDTO.CompanyId = Id;
        countryCreateDTO.Name = "Україна";
    }

    private async Task<Guid> CreateClientPrivateDataAsync(Guid clientId)
    {
        clientPrivateDataCreateDTO.ClientId = clientId;
        Guid clientPrivateDataId = await ClientPrivateDataRequest.CreateAsync(clientPrivateDataCreateDTO);
        if (clientPrivateDataId == Guid.Empty)
        {
            Snackbar.Add("Особисті дані не створені", Severity.Error);
        }

        Snackbar.Add("Особисті дані створені", Severity.Success);
        return clientPrivateDataId;
    }
    
    private async Task<Guid> CreateClientStatusHistoryAsync(Guid clientId)
    {
        clientStatusHistoryCreateDTO.ClientId = clientId;
        clientStatusHistoryCreateDTO.ClientStatus = ClientStatus.Active;
        clientStatusHistoryCreateDTO.DateTime = DateTime.UtcNow; 
        Guid clientStatusHistoryId = await ClientStatusHistoryRequest.CreateAsync(clientStatusHistoryCreateDTO);
        if (clientStatusHistoryId == Guid.Empty)
        {
            Snackbar.Add("Статус не змінено", Severity.Error);
        }
        return clientStatusHistoryId;
    }

    private void Cancel() => MudDialog.Cancel();
}