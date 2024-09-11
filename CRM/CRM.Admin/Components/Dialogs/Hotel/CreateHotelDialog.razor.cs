using CRM.Admin.Components.Dialogs.RoomType;
using CRM.Admin.Data.HotelDto;
using CRM.Admin.Data.RoomTypeDto;
using CRM.Admin.Requests.HotelRequests;
using CRM.Admin.Requests.RoomTypeRequests;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Dialogs.Hotel;

public partial class CreateHotelDialog
{
    [Inject] private IHotelRequest HotelRequest { get; set; } = null!;
    [Inject] private IRoomTypeRequest RoomTypeRequest { get; set; } = null!;
    [Inject] private ISnackbar? Snackbar { get; set; }
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

    private HotelCreateDto _hotelCreateDto { get; set; } = new();
    private List<RoomTypeDto> _roomTypeDtos = new();

    protected override async Task OnInitializedAsync()
    {
        _roomTypeDtos = await RoomTypeRequest.GetAllAsync();
    }

    private async Task Create()
    {
        try
        {
            _hotelCreateDto.CompanyId = Id;
            var hotelId = await HotelRequest.CreateAsync(_hotelCreateDto);
            if (hotelId != Guid.Empty)
            {
                Snackbar.Add("Додано заклад проживання", Severity.Success);
                MudDialog.Close(DialogResult.Ok(new HotelDto { Id = hotelId, Name = _hotelCreateDto.Name }));
            }
            else
            {
                Snackbar.Add($"Помилка при додаванні закладу проживання", Severity.Error);
            }
        }
        catch
        {
            Snackbar.Add($"Помилка при додаванні закладу проживання", Severity.Error);
        }
    }

    private async Task AddRoomType()
    {
        var parameters = new DialogParameters { { "Id", Id } };
        _dialogOptions.MaxWidth = MaxWidth.ExtraSmall;
        var dialogReference = await DialogService.ShowAsync<CreateRoomTypeDialog>("", parameters, _dialogOptions);
        var dialogResult = await dialogReference.Result;

        if (!dialogResult.Canceled && dialogResult.Data is RoomTypeDto newRoomType)
        {
            _roomTypeDtos.Add(newRoomType);
        }
    }

    private async Task<IEnumerable<Guid>> SearchRoomType(string value)
    {
        var filteredRoomTypes = await RoomTypeRequest.GetFiltredDataAsync(value);
        return filteredRoomTypes.Select(h => h.Id);
    }

    private string GetRoomTypeName(Guid id)
    {
        var roomType = _roomTypeDtos.FirstOrDefault(c => c.Id == id);
        return roomType?.Name ?? string.Empty;
    }

    void Cancel() => MudDialog.Cancel();
}