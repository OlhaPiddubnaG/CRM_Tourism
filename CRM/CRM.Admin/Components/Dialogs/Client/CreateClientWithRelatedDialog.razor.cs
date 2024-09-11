using CRM.Admin.Data.ClientDto;
using CRM.Admin.Data.CountryDto;
using CRM.Admin.Data.PassportInfoDto;
using CRM.Admin.Data.UserDto;
using CRM.Admin.Requests.ClientRequests;
using CRM.Admin.Requests.CountryRequests;
using CRM.Admin.Requests.UserRequests;
using CRM.Domain.Enums;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Dialogs.Client;

public partial class CreateClientWithRelatedDialog
{
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IClientRequest ClientRequest { get; set; } = null!;
    [Inject] private ICountryRequest CountryRequest { get; set; } = null!;
    [Inject] private IUserRequest UserRequest { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public Guid Id { get; set; }

    private List<UserDto> _managers = new();
    private readonly ClientCreateDto _clientCreateDto = InitializeClientCreateDto();
    private DateTime? _dateOfBirth = DateTime.UtcNow;
    private DateTime? _passportInternalExpiryDate = DateTime.UtcNow;
    private DateTime? _passportInternalIssueDate = DateTime.UtcNow;
    private DateTime? _passportInternationalExpiryDate = DateTime.UtcNow;
    private DateTime? _passportInternationalIssueDate = DateTime.UtcNow;
    private int _activeTabIndex = 0;

    protected override async Task OnInitializedAsync()
    {
        _managers = await UserRequest.GetAllAsync();
    }

    private async Task CreateClientAsync()
    {
        if (!IsClientDataValid())
        {
            Snackbar.Add("Будь ласка, заповніть всі поля", Severity.Error);
            return;
        }

        await PrepareClientDataAsync();
        var result = await ClientRequest.CreateClientWithRelatedAsync(_clientCreateDto);

        if (result.Success)
        {
            Snackbar.Add("Клієнта створено", Severity.Success);
            NavigateToClientPage();
        }
        else
        {
            Snackbar.Add("Помилка при створенні клієнта", Severity.Error);
        }
    }

    private bool IsClientDataValid() =>
        !string.IsNullOrEmpty(_clientCreateDto.Name) && !string.IsNullOrEmpty(_clientCreateDto.Surname);

    private static ClientCreateDto InitializeClientCreateDto() => new()
    {
        PassportsCreateDtos = new PassportInfoCreateDto[]
        {
            new PassportInfoCreateDto(),
            new PassportInfoCreateDto()
        }
    };

    private async Task PrepareClientDataAsync()
    {
        var countryCreateDto = new CountryCreateDto
        {
            CompanyId = Id,
            Name = "Україна"
        };

        _clientCreateDto.CompanyId = Id;
        _clientCreateDto.CountryId = await GetOrCreateCountryAsync(countryCreateDto);
        _clientCreateDto.DateOfBirth =
            _dateOfBirth.HasValue ? DateOnly.FromDateTime(_dateOfBirth.Value) : (DateOnly?)null;

        _clientCreateDto.PassportsCreateDtos[0].ExpiryDate = _passportInternalExpiryDate.HasValue
            ? DateOnly.FromDateTime(_passportInternalExpiryDate.Value)
            : (DateOnly?)null;
        _clientCreateDto.PassportsCreateDtos[0].DateOfIssue = _passportInternalIssueDate.HasValue
            ? DateOnly.FromDateTime(_passportInternalIssueDate.Value)
            : (DateOnly?)null;
        _clientCreateDto.PassportsCreateDtos[0].PassportType = PassportType.Internal;

        _clientCreateDto.PassportsCreateDtos[1].ExpiryDate = _passportInternationalExpiryDate.HasValue
            ? DateOnly.FromDateTime(_passportInternationalExpiryDate.Value)
            : (DateOnly?)null;
        _clientCreateDto.PassportsCreateDtos[1].DateOfIssue = _passportInternationalIssueDate.HasValue
            ? DateOnly.FromDateTime(_passportInternationalIssueDate.Value)
            : (DateOnly?)null;
        _clientCreateDto.PassportsCreateDtos[1].PassportType = PassportType.International;
    }

    private async Task<Guid> GetOrCreateCountryAsync(CountryCreateDto countryCreateDto)
    {
        var existingCountry = await CountryRequest.GetByNameAsync(countryCreateDto.Name);
        return existingCountry?.Id ?? await CountryRequest.CreateAsync(countryCreateDto);
    }


    private void NavigateToClientPage()
    {
        NavigationManager.NavigateTo("/clients");
        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}