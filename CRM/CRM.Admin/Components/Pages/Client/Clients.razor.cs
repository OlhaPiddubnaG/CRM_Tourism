using CRM.Admin.Components.Dialogs.Client;
using CRM.Admin.Data.ClientDto;
using CRM.Admin.Requests.ClientRequests;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Pages.Client;

public partial class Clients
{
    [Inject] private IDialogService DialogService { get; set; } = null!;
    [Inject] private IClientRequest ClientRequest { get; set; } = null!;

    private IEnumerable<ClientDto> _pagedData = null!;
    private MudTable<ClientDto> _table = null!;

    private readonly DialogOptions _dialogOptions = new()
    {
        CloseOnEscapeKey = true,
        CloseButton = true,
        DisableBackdropClick = true,
        MaxWidth = MaxWidth.ExtraSmall,
        FullWidth = true
    };

    private int _totalItems;
    private string _searchString = String.Empty;
    private Guid? _id;

    private async Task<TableData<ClientDto>> ServerReload(TableState state)
    {
        var requestParameters = new ClientRequestParameters
        {
            SearchString = _searchString,
            SortLabel = state.SortLabel,
            SortDirection = state.SortDirection,
            PageIndex = state.Page,
            PageSize = state.PageSize
        };

        var response = await ClientRequest.GetPagedDataAsync(requestParameters);

        _pagedData = response.Items;
        _totalItems = response.TotalItems;

        return new TableData<ClientDto>
        {
            TotalItems = _totalItems,
            Items = _pagedData
        };
    }

    private void OnSearch(string text)
    {
        _searchString = text;
        _table.ReloadServerData();
    }

    private async Task AddComment(Guid clientId)
    {
        var parameters = new DialogParameters { { "Id", clientId } };
        var dialogReference =
            await DialogService.ShowAsync<CreateCommentForClientDialog>("", parameters, _dialogOptions);
        var dialogResult = await dialogReference.Result;

        if (dialogResult.Canceled)
            return;

        await _table.ReloadServerData();
    }

    private async Task AddManager(Guid clientId)
    {
        var parameters = new DialogParameters { { "Id", clientId } };
        var dialogReference = await DialogService.ShowAsync<AddManagerForClientDialog>("", parameters, _dialogOptions);
        var dialogResult = await dialogReference.Result;

        if (dialogResult.Canceled)
            return;

        await _table.ReloadServerData();
    }

    private async Task AddStatus(Guid clientId)
    {
        var parameters = new DialogParameters { { "Id", clientId } };
        var dialogReference = await DialogService.ShowAsync<AddStatusForClientDialog>("", parameters, _dialogOptions);
        var dialogResult = await dialogReference.Result;

        if (dialogResult.Canceled)
            return;

        await _table.ReloadServerData();
    }
}