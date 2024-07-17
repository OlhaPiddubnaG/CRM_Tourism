using MudBlazor;

namespace CRM.Admin.Data.ClientDto;

public class ClientRequestParameters
{
    public string? SearchString { get; set; }
    public string? SortLabel { get; set; }
    public SortDirection SortDirection { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}