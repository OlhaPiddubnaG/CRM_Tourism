using MudBlazor;

namespace CRM.Admin.Data.OrderDto;

public class FilteredOrdersRequestParameters
{
    public Guid ClientId { get; set; }
    public string? SearchString { get; set; }
    public string? SortLabel { get; set; }
    public SortDirection SortDirection { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}