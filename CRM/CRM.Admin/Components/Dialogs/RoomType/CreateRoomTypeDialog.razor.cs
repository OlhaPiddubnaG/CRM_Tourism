using CRM.Admin.Data.RoomTypeDto;
using CRM.Admin.Requests.RoomTypeRequests;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Dialogs.RoomType;

public partial class CreateRoomTypeDialog
{
    [Inject] private IRoomTypeRequest RoomTypeRequest { get; set; } = null!;
    [Inject] private ISnackbar? Snackbar { get; set; }

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public Guid Id { get; set; }

    private RoomTypeCreateDto _roomTypeCreateDto { get; set; } = new();

    private async Task Create()
    {
        _roomTypeCreateDto.CompanyId = Id;
        var roomTypeId = await RoomTypeRequest.CreateAsync(_roomTypeCreateDto);
        if (roomTypeId != Guid.Empty)
        {
            Snackbar.Add("Додано тип номеру", Severity.Success);
            MudDialog.Close(DialogResult.Ok(new RoomTypeDto { Id = roomTypeId, Name = _roomTypeCreateDto.Name }));
        }
        else
        {
            Snackbar.Add($"Помилка при додаванні типу номеру", Severity.Error);
        }
    }

    void Cancel() => MudDialog.Cancel();
}