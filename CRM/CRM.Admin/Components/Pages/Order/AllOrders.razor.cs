using CRM.Admin.Data.OrderDto;
using CRM.Admin.Requests.OrderRequests;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Pages.Order;

public partial class AllOrders
{
    [Inject] private IDialogService DialogService { get; set; } = null!;
    [Inject] private IOrderRequest OrderRequest { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    private IEnumerable<OrderDto> _pagedData = null!;
    private MudTable<OrderDto> _table = null!;

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
 
    private async Task<TableData<OrderDto>> ServerReload(TableState state)
    {
        var requestParameters = new OrderRequestParameters
        {
            SearchString = _searchString,
            SortLabel = state.SortLabel,
            SortDirection = state.SortDirection,
            PageIndex = state.Page,
            PageSize = state.PageSize
        };

        var response = await OrderRequest.GetPagedDataAsync(requestParameters);

        _pagedData = response.Items;
        _totalItems = response.TotalItems;

        return new TableData<OrderDto>
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

    private void CreateOrder()
    {
        NavigationManager.NavigateTo("/newOrder");
    }
}

