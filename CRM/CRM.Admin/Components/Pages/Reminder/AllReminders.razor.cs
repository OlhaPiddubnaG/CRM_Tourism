using CRM.Admin.Data.UserTasksDto;
using CRM.Admin.Requests.UserTasksRequests;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Pages.Reminder;

public partial class AllReminders
{
    [Inject] private IUserTasksRequest UserTasksRequest { get; set; } = null!;
    
    [Parameter] public string userId { get; set; } = null!;
    
    private IEnumerable<UserTasksDto> _pagedData = null!;
    private MudTable<UserTasksDto> _table = null!;
    private int _totalItems;
    private string _searchString = String.Empty;
    private Guid _id;

    protected override async Task OnInitializedAsync()
    {
        _id = Guid.Parse(userId);
    }
    private async Task<TableData<UserTasksDto>> ServerReload(TableState state)
    {
        var requestParameters = new FilteredTasksRequestParameters
        {
            UserId = _id,
            SearchString = _searchString,
            SortLabel = state.SortLabel,
            SortDirection = state.SortDirection,
            PageIndex = state.Page,
            PageSize = state.PageSize
        };

        var response = await UserTasksRequest.GetPagedDataAsync(requestParameters);

        _pagedData = response.Items;
        _totalItems = response.TotalItems;

        return new TableData<UserTasksDto>
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