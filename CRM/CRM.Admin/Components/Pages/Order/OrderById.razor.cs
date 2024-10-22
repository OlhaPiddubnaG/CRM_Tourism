using CRM.Admin.Data.OrderDto;
using CRM.Admin.Requests.OrderRequests;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Pages.Order;

public partial class OrderById
{
    [Inject] private IOrderRequest OrderRequest { get; set; } = null!;

    [Parameter] public required string ClientId { get; set; } 
    
    private IEnumerable<OrderDto> _pagedData = null!;
    private MudTable<OrderDto> _table = null!;
    
    private int _totalItems;
    private string _searchString = String.Empty;
    private Guid _id;

    protected override async Task OnInitializedAsync()
    {
        _id = Guid.Parse(ClientId);
    }

    private async Task<TableData<OrderDto>> ServerReload(TableState state)
    {
        var requestParameters = new FilteredOrdersRequestParameters
        {
            ClientId =  _id,
            SearchString = _searchString,
            SortLabel = state.SortLabel,
            SortDirection = state.SortDirection,
            PageIndex = state.Page,
            PageSize = state.PageSize
        };

        var response = await OrderRequest.GetPagedDataByClientIdAsync(requestParameters);

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
}
