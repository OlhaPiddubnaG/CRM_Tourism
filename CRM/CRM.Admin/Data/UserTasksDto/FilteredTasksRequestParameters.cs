using MudBlazor;

namespace CRM.Admin.Data.UserTasksDto;

public class FilteredTasksRequestParameters
{
    public Guid UserId { get; set; }
    public string? SearchString { get; set; }
    public string? SortLabel { get; set; }
    public SortDirection SortDirection { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}