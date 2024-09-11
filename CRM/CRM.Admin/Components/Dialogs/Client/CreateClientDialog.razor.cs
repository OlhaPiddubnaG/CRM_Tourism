using CRM.Admin.Components.Dialogs.Country;
using CRM.Admin.Data.ClientDto;
using CRM.Admin.Data.CountryDto;
using CRM.Admin.Data.UserDto;
using CRM.Admin.Requests.ClientRequests;
using CRM.Admin.Requests.CountryRequests;
using CRM.Admin.Requests.UserRequests;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Dialogs.Client;

public partial class CreateClientDialog
{
    [Inject] private IClientRequest ClientRequest { get; set; } = null!;
    [Inject] private ICountryRequest CountryRequest { get; set; } = null!;
    [Inject] private IUserRequest UserRequest { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] IDialogService DialogService { get; set; } = default!;

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public Guid Id { get; set; }

    private DialogOptions _dialogOptions = new()
    {
        CloseOnEscapeKey = true,
        CloseButton = true,
        DisableBackdropClick = true,
        FullWidth = true
    };

    private List<UserDto> _managers = new();
    private readonly ClientCreateDto _clientCreateDto = new();
    private List<CountryDto> _countriesDtos = new();
    private DateTime? _dateOfBirth = DateTime.UtcNow;

    protected override async Task OnInitializedAsync()
    {
        _managers = await UserRequest.GetAllAsync();
        _countriesDtos = await CountryRequest.GetAllAsync();
    }

    private async Task CreateClientAsync()
    {
        if (!IsClientDataValid())
        {
            Snackbar.Add("Будь ласка, заповніть всі поля", Severity.Error);
            return;
        }

        var countryCreateDto = new CountryCreateDto
        {
            CompanyId = Id,
            Name = "Україна"
        };
        _clientCreateDto.CompanyId = Id;
        _clientCreateDto.DateOfBirth =
            _dateOfBirth.HasValue ? DateOnly.FromDateTime(_dateOfBirth.Value) : (DateOnly?)null;
        Guid clientId = await ClientRequest.CreateAsync(_clientCreateDto);

        if (clientId != Guid.Empty)
        {
            Snackbar.Add("Клієнта створено", Severity.Success);
            MudDialog.Close(DialogResult.Ok(new ClientDto
                { Id = clientId, Name = _clientCreateDto.Name, Surname = _clientCreateDto.Surname }));
        }
        else
        {
            Snackbar.Add("Помилка при створенні клієнта", Severity.Error);
        }
    }

    private bool IsClientDataValid() =>
        !string.IsNullOrEmpty(_clientCreateDto.Name) && !string.IsNullOrEmpty(_clientCreateDto.Surname);

    private async Task AddCountry()
    {
        var parameters = new DialogParameters { { "Id", Id } };
        _dialogOptions.MaxWidth = MaxWidth.ExtraSmall;
        var dialogReference = await DialogService.ShowAsync<CreateCountryDialog>("", parameters, _dialogOptions);
        var dialogResult = await dialogReference.Result;

        if (!dialogResult.Canceled && dialogResult.Data is CountryDto newCountry)
        {
            _countriesDtos.Add(newCountry);
        }
    }

    private string GetCountryName(Guid id)
    {
        var country = _countriesDtos.FirstOrDefault(c => c.Id == id);
        return country?.Name ?? string.Empty;
    }

    private async Task<IEnumerable<Guid>> SearchCountries(string value)
    {
        var filteredCountries = await CountryRequest.GetFiltredDataAsync(value);
        return filteredCountries.Select(c => c.Id);
    }

    private void Cancel() => MudDialog.Cancel();
}