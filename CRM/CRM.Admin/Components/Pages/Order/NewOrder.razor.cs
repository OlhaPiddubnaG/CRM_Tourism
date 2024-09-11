using CRM.Admin.Auth;
using CRM.Admin.Components.Dialogs.Client;
using CRM.Admin.Components.Dialogs.Country;
using CRM.Admin.Components.Dialogs.Hotel;
using CRM.Admin.Components.Dialogs.Reminder;
using CRM.Admin.Components.Dialogs.Touroperator;
using CRM.Admin.Data.ClientDto;
using CRM.Admin.Data.CountryDto;
using CRM.Admin.Data.HotelDto;
using CRM.Admin.Data.MealsDto;
using CRM.Admin.Data.NumberOfPeopleDto;
using CRM.Admin.Data.OrderDto;
using CRM.Admin.Data.PaymentDto;
using CRM.Admin.Data.StaysDto;
using CRM.Admin.Data.TouroperatorDto;
using CRM.Admin.Requests.ClientRequests;
using CRM.Admin.Requests.CountryRequests;
using CRM.Admin.Requests.HotelRequests;
using CRM.Admin.Requests.OrderRequests;
using CRM.Admin.Requests.TouroperatorRequests;
using CRM.Domain.Enums;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Pages.Order;

public partial class NewOrder
{
    [Inject] private IOrderRequest OrderRequest { get; set; } = null!;
    [Inject] private IClientRequest ClientRequest { get; set; } = null!;
    [Inject] private ICountryRequest CountryRequest { get; set; } = null!;
    [Inject] private ITouroperatorRequest TouroperatorRequest { get; set; } = null!;
    [Inject] private IHotelRequest HotelRequest { get; set; } = null!;
    [Inject] private AuthState AuthState { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] IDialogService DialogService { get; set; } = default!;


    private DialogOptions _dialogOptions = new()
    {
        CloseOnEscapeKey = true,
        CloseButton = true,
        DisableBackdropClick = true,
        FullWidth = true
    };

    private OrderCreateDto _orderCreateDto = new();
    private NumberOfPeopleCreateDto _adultCreateDto = new();
    private NumberOfPeopleCreateDto _childCreateDto = new();
    private PaymentCreateDto _paymentCreateDto = new();
    private StaysCreateDto _staysCreateDto = new();
    private MealsCreateDto _mealsCreateDto = new();
    private List<CountryDto> _countriesDtos = new();
    private List<TouroperatorDto> _touroperatorDtos = new();
    private List<ClientDto> _clientDtos = new();
    private List<HotelDto> _hotelDtos = new();

    private MudForm _form;
    private Guid _userId;
    private Guid _companyId;
    private DateTime? _paymentData = DateTime.UtcNow;
    private DateTime? _selectedDate = DateTime.Today;
    private bool isValid;
 

    protected override async Task OnInitializedAsync()
    {
        _userId = AuthState.UserId;
        _companyId = AuthState.CompanyId;
        _countriesDtos = await CountryRequest.GetAllAsync();
        _touroperatorDtos = await TouroperatorRequest.GetAllAsync();
        _clientDtos = await ClientRequest.GetAllAsync();
        _hotelDtos = await HotelRequest.GetAllAsync();
    }

    private async Task CreateOrder()
    {
        if (isValid)
        {
            try
            {
                _orderCreateDto.CompanyId = _companyId;
                _orderCreateDto.UserId = _userId;

                var numberOfPeopleList = new List<NumberOfPeopleCreateDto>();

                if (_adultCreateDto.Number > 0)
                {
                    numberOfPeopleList.Add(new NumberOfPeopleCreateDto
                    {
                        ClientCategory = ClientCategory.Adult,
                        Number = _adultCreateDto.Number
                    });
                }

                if (_childCreateDto.Number > 0)
                {
                    numberOfPeopleList.Add(new NumberOfPeopleCreateDto
                    {
                        ClientCategory = ClientCategory.Child,
                        Number = _childCreateDto.Number
                    });
                }

                _orderCreateDto.NumberOfPeopleCreateDto = numberOfPeopleList.ToArray();
                _orderCreateDto.StaysCreateDto = new[] { _staysCreateDto };
                _mealsCreateDto.HotelId = _staysCreateDto.HotelId;
                _orderCreateDto.MealsCreateDto = new[] { _mealsCreateDto };
                _paymentCreateDto.DateTime = _paymentData;
                _orderCreateDto.PaymentCreateDto = new[] { _paymentCreateDto };

                var orderId = await OrderRequest.CreateOrderWithRelatedAsync(_orderCreateDto);
                if (orderId.Success)
                {
                    Snackbar.Add("Замовлення створено", Severity.Success);
                }
                else
                {
                    Snackbar.Add("Помилка при створенні замовлення", Severity.Error);
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add("An error occurred while creating the order.", Severity.Error);
            }
        }
        else
        {
            Snackbar.Add("An error occurred while creating the order.", Severity.Error);
        }
    }

    private async Task<IEnumerable<Guid>> SearchCountries(string value)
    {
        var filteredCountries = await CountryRequest.GetFiltredDataAsync(value);
        return filteredCountries.Select(c => c.Id);
    }

    private async Task<IEnumerable<Guid>> SearchClients(string value)
    {
        var filteredClients = await ClientRequest.GetFiltredDataAsync(value);
        return filteredClients.Select(t => t.Id);
    }

    private async Task<IEnumerable<Guid>> SearchStays(string value)
    {
        var filteredHotels = await HotelRequest.GetFiltredDataAsync(value);
        return filteredHotels.Select(h => h.Id);
    }

    private async Task<IEnumerable<Guid>> SearchTouroperators(string value)
    {
        var filteredTouroperators = await TouroperatorRequest.GetFiltredDataAsync(value);
        return filteredTouroperators.Select(t => t.Id);
    }


    private string GetCountryName(Guid id)
    {
        var country = _countriesDtos.FirstOrDefault(c => c.Id == id);
        return country?.Name ?? string.Empty;
    }

    private string GetClientFullName(Guid id)
    {
        var client = _clientDtos.FirstOrDefault(c => c.Id == id);
        if (client != null)
            return $"{client.Name} {client.Surname}";
        else
        {
            return string.Empty;
        }
    }

    private string GetStaysName(Guid id)
    {
        var stay = _hotelDtos.FirstOrDefault(c => c.Id == id);
        return stay?.Name ?? string.Empty;
    }

    private string GetTouroperatorName(Guid id)
    {
        var touroperator = _touroperatorDtos.FirstOrDefault(c => c.Id == id);
        return touroperator?.Name ?? string.Empty;
    }

    private async Task AddCountry()
    {
        var parameters = new DialogParameters { { "Id", _companyId } };
        _dialogOptions.MaxWidth = MaxWidth.ExtraSmall;
        var dialogReference = await DialogService.ShowAsync<CreateCountryDialog>("", parameters, _dialogOptions);
        var dialogResult = await dialogReference.Result;

        if (!dialogResult.Canceled && dialogResult.Data is CountryDto newCountry)
        {
            _countriesDtos.Add(newCountry);
        }
    }

    private async Task AddClient()
    {
        var parameters = new DialogParameters { { "Id", _companyId } };
        _dialogOptions.MaxWidth = MaxWidth.Small;
        var dialogReference = await DialogService.ShowAsync<CreateClientDialog>("", parameters, _dialogOptions);
        var dialogResult = await dialogReference.Result;

        if (!dialogResult.Canceled && dialogResult.Data is ClientDto newClient)
        {
            _clientDtos.Add(newClient);
        }
    }

    private async Task AddStays()
    {
        var parameters = new DialogParameters { { "Id", _companyId } };
        _dialogOptions.MaxWidth = MaxWidth.Small;
        var dialogReference = await DialogService.ShowAsync<CreateHotelDialog>("", parameters, _dialogOptions);
        var dialogResult = await dialogReference.Result;

        if (!dialogResult.Canceled && dialogResult.Data is HotelDto newHotel)
        {
            _hotelDtos.Add(newHotel);
        }
    }

    private async Task AddTouroperator()
    {
        var parameters = new DialogParameters { { "Id", _companyId } };
        _dialogOptions.MaxWidth = MaxWidth.ExtraSmall;
        var dialogReference = await DialogService.ShowAsync<CreateTouroperatorDialog>("", parameters, _dialogOptions);
        var dialogResult = await dialogReference.Result;

        if (!dialogResult.Canceled && dialogResult.Data is TouroperatorDto newTouroperator)
        {
            _touroperatorDtos.Add(newTouroperator);
        }
    }
    
    private async Task CreateReminder()
    {
        var parameters = new DialogParameters { { "Id", _userId } };
        _dialogOptions.MaxWidth = MaxWidth.Small;
        var dialogReference = await DialogService.ShowAsync<CreateReminderDialog>("",parameters, _dialogOptions);

        var dialogResult = await dialogReference.Result;

        if (dialogResult.Canceled)
            return;
    }
}